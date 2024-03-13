
# Documentación de Scripts para Juego con Firebase Realtime Database

Este documento explica el funcionamiento y la interacción entre tres scripts diseñados para un juego en Unity, donde un jugador recolecta objetos y su información se actualiza en Firebase Realtime Database.

## CameraController.cs

Este script se encarga de controlar la cámara en el juego para que siga al jugador manteniendo un offset (desplazamiento) constante.

**Variables principales:**

- `player`: Referencia al objeto del jugador que la cámara debe seguir.
- `offset`: Diferencia de posición entre la cámara y el jugador para mantener constante.

**Métodos principales:**

- `Start()`: Calcula el offset inicial entre la cámara y el jugador.
- `LateUpdate()`: Actualiza la posición de la cámara basándose en la del jugador más el offset.

## PlayerController.cs

Controla las acciones y movimientos del jugador, maneja el input para moverlo y detecta colisiones con objetos "PickUp" para incrementar la puntuación. Interactúa con `Realtime.cs` para actualizar la puntuación y el récord en Firebase.

**Variables principales:**

- Variables para el movimiento del jugador y su velocidad.
- `scoreCount` y `record`: Para manejar la puntuación actual y el récord.
- Referencia a `Realtime` para interactuar con Firebase.

**Métodos principales:**

- `Start()`: Inicializa componentes y solicita el récord actual de Firebase.
- `FixedUpdate()`: Aplica el movimiento al jugador basándose en inputs del usuario.
- `OnTriggerEnter(Collider other)`: Detecta colisiones con objetos "PickUp", aumenta la puntuación y actualiza Firebase.

## Realtime.cs

Central para la interacción con Firebase Realtime Database. Gestiona la conexión, la creación de prefabs basados en datos de Firebase, y actualiza información del jugador como la puntuación y el récord.

**Variables principales:**

- Variables para la conexión a Firebase, referencias a la base de datos y a colecciones específicas como "Jugadores" y "Prefabs".
- Identificador del usuario y referencias para actualizar datos del jugador en Firebase.

**Métodos principales:**

- `Conexion()`: Establece la conexión con Firebase.
- `Start()`: Conexión inicial, obtiene datos de prefabs de Firebase, registra el dispositivo/jugador.
- `AñadirPrefabs(DataSnapshot snapshot)`: Crea objetos en el juego basados en información de Firebase.
- `AltaDevice()`: Registra un nuevo nodo en Firebase con identificador único para el dispositivo.
- `GetRecord(Action<int> callback)`: Obtiene el récord actual del jugador de Firebase.
- `UpdateScore(int scoreCount)`: Actualiza la puntuación del jugador en Firebase.
- `UpdateRecord(int scoreCount)`: Actualiza el récord del jugador en Firebase si se supera la puntuación previa.

Estos scripts ofrecen una estructura básica para un juego donde el jugador recolecta objetos y su puntuación y récord se actualizan en una base de datos en tiempo real, facilitando competencias entre jugadores o la persistencia de datos entre sesiones.

