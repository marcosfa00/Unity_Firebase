using System.Collections;
using System;
using UnityEngine;

using Firebase;
using Firebase.Database;
using Firebase.Extensions;
using Unity.VisualScripting;
using System.Threading.Tasks;

public class Realtime : MonoBehaviour
{
    // conexion con Firebase (_app(para decir que es privada))
    private FirebaseApp _app;
    // Singleton de la Base de Datos
    private FirebaseDatabase _db;
    // referencia a la 'coleccion' Clientes
    private DatabaseReference _refClientes;
    // GameObject a modificar
    public GameObject ondavital;
    // PickUps
    public GameObject pickUp;
    // referencia a la colección de los pickUps
    private DatabaseReference _refPickUp;

    // referencia al usuario
    private string _userId;
    // referencia base de datos del usuario
    private DatabaseReference _refUser;
    private bool _refUserCreated = false;
    

    
    // Start is called before the first frame update
    void Start()
    {
        // realizamos la conexion a Firebase
        _app = Conexion();
        
        // obtenemos el Singleton de la base de datos
        _db = FirebaseDatabase.DefaultInstance;
        
        // Obtenemos la referencia a TODA la base de datos
        // DatabaseReference reference = db.RootReference;
        
        // Definimos la referencia a Clientes
        _refClientes = _db.GetReference("Jugadores");

        // Definimos la referencia a PickUps
        _refPickUp = _db.GetReference("Prefabs");

        // Recogemos todos los valoresd de las posiciones del prefab y las creamos
        _refPickUp.GetValueAsync().ContinueWithOnMainThread(task => {
                if(task.IsFaulted) {
                    // error
                } else if(task.IsCompleted){
                    DataSnapshot snapshot = task.Result;
                    AñadirPrefabs(snapshot);
                }
            }
        );

        // Añadimos un nodo
        AltaDevice();

        _refUser = _db.GetReference("Jugadores").Child(_userId);
        _refUserCreated = true;
    }
    
    // realizamos la conexion a Firebase
    // devolvemos una instancia de esta aplicacion
    FirebaseApp Conexion()
    {
        FirebaseApp firebaseApp = null;
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWithOnMainThread(task =>
        {
            var dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
            {
                // Create and hold a reference to your FirebaseApp,
                // where app is a Firebase.FirebaseApp property of your application class.
                firebaseApp = FirebaseApp.DefaultInstance;
                // Set a flag here to indicate whether Firebase is ready to use by your app.
            }
            else
            {
                Debug.LogError(System.String.Format(
                    "Could not resolve all Firebase dependencies: {0}", dependencyStatus));
                // Firebase Unity SDK is not safe to use here.
                firebaseApp = null;
            }
        });
            
        return firebaseApp;
    }

    void AñadirPrefabs(DataSnapshot snapshot){
        foreach(var resultado in snapshot.Children) {
            var x = 0f;
            var y = 0f;
            var z = 0f;
            Debug.LogFormat("Key = {0}", resultado.Key);
            foreach(var item in resultado.Children) {
                Debug.LogFormat("(key){0}:(value){1}", item.Key, item.Value);
                if(item.Key == "x"){
                    float.TryParse(item.Value.ToString(), out x);
                    Debug.LogFormat("MI XXXXXXXXXXXXX =  {0}", x);
                } else if(item.Key == "y"){
                    float.TryParse(item.Value.ToString(), out y);
                    Debug.LogFormat("MI YYYYYYYYYYYYY = {0}", y);
                } else if(item.Key == "z") {
                    float.TryParse(item.Value.ToString(), out z);
                    Debug.LogFormat("MI ZZZZZZZZZZZ = {0}", z);
                } else{
                    Debug.LogFormat("aaaaaaaaaaaa");
                }
            }
            Vector3 spawnPosition = new Vector3(x,y,z);
            // instanciamos el pickUp en la posicion
            // prefab/objeto - Vector3 - Rotation
            Instantiate(pickUp, spawnPosition, pickUp.transform.rotation);
        }
    }


    // doy de alta un nodo con un identificador unico
    void AltaDevice() {
        _userId = SystemInfo.deviceUniqueIdentifier;
        _refClientes.Child(_userId).Child("nombre").SetValueAsync("Mi dispositivo");
    }
    
    // Update is called once per frame
    void Update()
    {
    }


    public IEnumerator GetRecord(Action<int> callback) {
        while(!_refUserCreated){
            yield return null;
        }
        var task = _refUser.Child("Record").GetValueAsync();
            {
                int record = 0;
                yield return new WaitUntil(() => task.IsCompleted);
                if (task.IsFaulted)
                {
                    Debug.LogError("Error");
                }
                else if (task.IsCompleted)
                {
                    DataSnapshot snapshot = task.Result; 
                    record = int.Parse(snapshot.Value.ToString());


                }
                callback(record);
            }
    }

    public void UpdateScore(int scoreCount){
        _refUser.Child("Puntos").SetValueAsync(scoreCount);
    }

    public void UpdateRecord(int scoreCount){
        _refUser.Child("Record").SetValueAsync(scoreCount);
    }
}