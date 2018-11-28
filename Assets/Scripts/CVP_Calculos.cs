using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CVP_Calculos {
    public const string CENTRO_DO_MAPA = "UTFPR";
    public const float CENTRO_DO_MAPA__LATITUDE = -25.4392969F;
    public const float CENTRO_DO_MAPA__LONGITUDE = -49.2688762F;
    public const float RAIO_DA_TERRA = 6371000;
    public const float RAIO_DA_TERRA__POLAR = 6357000;
    public const float RAIO_DA_TERRA__EQUATORIAL = 6378000;

    public static float MenorValor_SteeringAngle(Biarticulado bi)
    {
        float result = float.NaN;
        foreach (VagaoBiarticulado v in bi.vagoes)
        {
            if (float.IsNaN(result) || (v.canSteer && result > v.maxSteeringAngle))
                result = v.maxSteeringAngle;
        }
        return result;
    }

    public static float DistanciaDeParada(Biarticulado bi)
    {
        float steeringAngle = MenorValor_SteeringAngle(bi);
        float result = bi.distanciaDeFrenagem;
        //Distancia para frenagem é proporcional à velocidade atual dividida pela distância de frenagem padrão.
        result *= (bi.velocidadeAtual / bi.velocidadeMaxima);
        //Mas caso o waypoint seguinte não seja uma parada de ônibus e não forme uma curva, a frenagem é reduzida.
        if (bi.wp_proximo && bi.wp_proximo.tipoWaypoint == CVP.ModuloOnibus.TipoWaypont.COMUM)
            result *= (Mathf.Clamp(bi.wps_angulo, 0, steeringAngle) / steeringAngle);
        return result;
    }

    public static Vector3 GPS_to_XYZ(float latitude, float longitude, float altura)      //TODO DECENTEMENTES
    {
        /*
         * A conversão de coordenadas GPS para XYZ é feita usando o cálculo do arco de circunferência entre os pontos:
         * [LAT;LON;ALT] do CENTRO_DO_MAPA
         * [LAT;LON;ALT] informados nos parâmetros desta função.
         * 
         * Os valores X (horizontal) e Z (vertical) recebem o resultado da distância entre os pontos.
         * Por hora, como todo o mapa é igual em altitude, todos recebem zero como valor Y.
         */
        Vector3 result = new Vector3();

        //Longitude para X
        result.x = (longitude - CENTRO_DO_MAPA__LONGITUDE);
        result.x *= Mathf.PI * RAIO_DA_TERRA / 180;

        //Latitude para Z
        result.z = (latitude - CENTRO_DO_MAPA__LATITUDE);
        result.z *= Mathf.PI * RAIO_DA_TERRA / 180;

        //Altitude para Y
        result.y = 0;

        return result;
    }

    public static bool AjustarPosicoesParaRotaPadrao(Rota rota, Vector3 posOriginal, float toleranciaDistanciaParaRota, out Vector3 posAjustada)
    {
        /*
        Basicamente:
        1.      Para cada Waypoint da Rota:
        1.1.    Gera uma reta (em unitês: Ray) a partir de cada 2 Waypoints consecutivos da rota.
        1.2.    Calcula a distância do ponto a ser ajustado para esta reta.
        1.3.    Se a distância for igual ou menor que distanciaMaxima
        1.3.1.  Armazena a reta gerada.
        1.3.2.  Redefine distanciaMaxima como sendo essa distância.
        //      TODO:   Ainda não há verificação se o ponto está realisticamente próximo dos Waypoints ou do trajeto entre os dois Waypoints
        2.      Se foi definida uma reta válida:
        2.1.    Calcula a posição ajustada para a reta encontrada.
        2.1.    Retorna TRUE
        3.      Senão
        3.1.    Retorna FALSE
        */

        Rota_Waypoint wpA, wpB;
        float dist_Found = float.NaN;
        Rota_Waypoint wpA_Found = null, wpB_Found = null;
        for (int i = 1; i <= rota.waypoints.Count; i++)
        {
            wpA = rota.waypoints[i - 1];
            if (i >= rota.waypoints.Count)
            {
                if (rota.waypointsEmCiclo)
                    wpB = rota.waypoints[0];
                else
                    break;
            }
            else
            {
                wpB = rota.waypoints[i];
            }

            //Para evitar de ficar comparando com pares de waypoints muito distantes da posição original informada.
            float dist_orig_To_wpA = Vector3.Distance(posOriginal, wpA.transform.position);
            float dist_orig_To_wpB = Vector3.Distance(posOriginal, wpB.transform.position);
            if (dist_orig_To_wpA <= toleranciaDistanciaParaRota && dist_orig_To_wpB <= toleranciaDistanciaParaRota)
            {
                float aux = DistanciaEntrePontoESegmentoDeReta(posOriginal, wpA.transform.position, wpB.transform.position);

                if (float.IsNaN(dist_Found) || aux < dist_Found)
                {
                    dist_Found = aux;
                    wpA_Found = wpA;
                    wpB_Found = wpB;
                }
            }
        }

        if (wpA_Found && wpB_Found)
        {
            float x = Mathf.Clamp(
                posOriginal.x,
                Mathf.Min(wpA_Found.transform.position.x, wpB_Found.transform.position.x),
                Mathf.Max(wpA_Found.transform.position.x, wpB_Found.transform.position.x));
            float y = Mathf.Clamp(
                posOriginal.y,
                Mathf.Min(wpA_Found.transform.position.y, wpB_Found.transform.position.y),
                Mathf.Max(wpA_Found.transform.position.y, wpB_Found.transform.position.y));
            float z = Mathf.Clamp(
                posOriginal.z,
                Mathf.Min(wpA_Found.transform.position.z, wpB_Found.transform.position.z),
                Mathf.Max(wpA_Found.transform.position.z, wpB_Found.transform.position.z));
            posAjustada = new Vector3(x, y, z);
            return true;
        }
        else
        {
            posAjustada = Vector3.zero;
            return false;
        }
    }

    //public float minimum_distance(vec2 v, vec2 w, vec2 p)
    //{
    //    // Return minimum distance between line segment vw and point p
    //    const float l2 = length_squared(v, w);  // i.e. |w-v|^2 -  avoid a sqrt
    //    if (l2 == 0.0) return distance(p, v);   // v == w case
    //                                            // Consider the line extending the segment, parameterized as v + t (w - v).
    //                                            // We find projection of point p onto the line. 
    //                                            // It falls where t = [(p-v) . (w-v)] / |w-v|^2
    //                                            // We clamp t from [0,1] to handle points outside the segment vw.
    //    const float t = max(0, min(1, dot(p - v, w - v) / l2));
    //    const vec2 projection = v + t * (w - v);  // Projection falls on the segment
    //    return distance(p, projection);
    //}

    public static float DistanciaEntrePontoESegmentoDeReta(Vector3 ponto, Vector3 segA, Vector3 segB)
    {
        if (segA == segB)
            return Vector3.Distance(ponto, segA);

        Vector3 direcao = segB - segA;
        float length = direcao.sqrMagnitude;

        float t = Mathf.Max(0, Mathf.Min(1, Vector3.Dot(ponto - segA, ponto - segB) / length));
        Vector3 projection = segA + (t * (segB - segA));

        return Vector3.Distance(ponto, projection);
    }
}
