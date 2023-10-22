    using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float speed = 0.8f;
    private Rigidbody2D rigidbody2D;
    [SerializeField] private AudioClip _audioClip;
    private AudioSource _audioSource;
    private Animator animator;
    Vector2 movement;
    Vector2 pos_inicial;
    bool inicio = true;
    private string user;
    private bool playing = false;
    int mejora_speed;
    private float audio = 0.0f;
    void Start()
    {
        audio = float.Parse(PlayerPrefs.GetString("volumeSFX"));
        _audioSource = GetComponent<AudioSource>();
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        user = PlayerPrefs.GetString("user");
        if (_audioSource == null)
        {
            Debug.LogError("The audio is null");
        }
        else
            _audioSource.clip = _audioClip;
            _audioSource.volume = audio;
            
    }

    public void setInitialPosition(string userr)
    {
        AdminMySQL adminMySQL = GameObject.Find("AdminSQL").GetComponent<AdminMySQL>();
        string selecter = "SELECT * FROM `posicion` WHERE `id_jgdr` LIKE '" + userr + "';";
        MySqlDataReader Resultado = adminMySQL.Select(selecter);
        Resultado.Read();
        float x1 = Resultado.GetFloat(2);
        float y1 = Resultado.GetFloat(3);
        Resultado.Close();
        pos_inicial.x = x1;
        pos_inicial.y = y1;
        transform.position = pos_inicial;
        inicio = false;
        adminMySQL.closeConnection();
        //Para la otra consulta
        adminMySQL.justOpenConnection();
        selecter = "SELECT `mejora_velocidad` FROM `mejora` WHERE `id_jgdr` LIKE '" + userr + "';";
        MySqlDataReader res = adminMySQL.Select(selecter);
        res.Read();
        mejora_speed = res.GetInt32(0);
        res.Close();
    }

    void Update()
    {
        if (inicio == true)
        {
            setInitialPosition(user);
        }

        if (Input.GetKey(KeyCode.LeftShift) && (mejora_speed == 1))
        {
            speed = 1.8f;
            _audioSource.pitch = 1.5f;
        }
        else
        {
            _audioSource.pitch = 1.2f;
            speed = 1.0f;
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);
        //Para el sonido de caminar
        if (movement.x != 0 || movement.y != 0)
        {
            if (!playing)
            {
                _audioSource.Play();
            }
            playing = true;
        }
        else
        {
            playing = false;
            _audioSource.Pause();
        }

        //Para el celular
        if (Input.GetKey(KeyCode.H))
        {
            animator.SetBool("phoneUse", true);
        }
        else
        {
            animator.SetBool("phoneUse", false);
        }
    }
    private void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + movement * speed * Time.deltaTime);
    }

}

/*
using MySql.Data.MySqlClient;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Movement : MonoBehaviour
{
    public float speed = 0.8f;
    public string scene_change;
    private new Rigidbody2D rigidbody2D;
    private Animator animator;
    Vector2 movement;
    Vector2 pos_inicial;
    bool inicio = true;
    private string user;
    int mejora_speed;
    public int num_mapa;
    void Start()
    {   
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        user = PlayerPrefs.GetString("user");
    }

    private void setInitialPosition(string userr)
    {
        
    }

    void Update()
    {
        if (inicio == true)
        {
            setInitialPosition(user);
        }

        if (Input.GetKey(KeyCode.LeftShift) && (mejora_speed == 1))
        {
            speed = 1.3f;
        }
        else
        {
            speed = 0.8f;
        }
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");

        animator.SetFloat("Horizontal", movement.x);
        animator.SetFloat("Vertical", movement.y);
        animator.SetFloat("speed", movement.sqrMagnitude);
        
    }
    private void FixedUpdate()
    {
        rigidbody2D.MovePosition(rigidbody2D.position + movement * speed * Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Conexiones_escenas scene = collision.gameObject.GetComponent<Conexiones_escenas>();

        if (scene != null)
        {
            LoadScene(scene_change);
        }
    }
    private void LoadScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);
    }
}*/