using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class Animacioncitas : MonoBehaviour
{
    public Image image;
    public GameObject villan;
    public RectTransform rect;
    private float speed = 3.0f; 
    private float amplitud = 1.0f; 

    public void moverInfinito()
    {
        rect.anchoredPosition = new Vector3(amplitud*20* Mathf.Sin(-Time.time * speed), amplitud * 10 * Mathf.Cos(Time.time * speed), 0);
    }

    public void girarYSalir()
    {
        float tiempoTotal = 3f;                 // duración total de la animación (en segundos)
        float tiempoActual = 0f;                // tiempo que ha pasado desde el inicio de la animación
        Vector3 escalaInicial = villan.transform.localScale;  // escala inicial del objeto

        while (tiempoActual < tiempoTotal)
        {
            tiempoActual += Time.deltaTime;

            // Girar el objeto
            villan.transform.Rotate(Vector3.up * 90 * Time.deltaTime);

            // Escalar el objeto
            float escala = 1 - tiempoActual / tiempoTotal;
            villan.transform.localScale = escala * escalaInicial;
        }
        gameObject.SetActive(false);
    }
    public void damaged_anim()
    {
        StartCoroutine(damaged());
    }
    IEnumerator damaged()
    {
        float tiempoTotal = 0.5f;
        float tiempoActual = 0f;
        speed = 30.0f;
        amplitud = 1.5f;
        image.GetComponent<Image>().color = new Color32(255, 136, 136, 255);
        while (tiempoActual < tiempoTotal)
        {
            tiempoActual += Time.deltaTime;
            yield return null;
        }
        image.GetComponent<Image>().color = new Color32(255, 255, 255, 255);
        speed = 3.0f;
        amplitud = 1.0f;
    }
}