# 🚀 MicroRush

![Unity](https://img.shields.io/badge/Unity-2022%2B-black?logo=unity)
![C#](https://img.shields.io/badge/C%23-Programming-blue?logo=csharp)
![Firebase](https://img.shields.io/badge/Firebase-Backend-orange?logo=firebase)
![UX/UI](https://img.shields.io/badge/UX%2FUI-Nielsen_Heuristics-success)

**MicroRush** es un videojuego 2D de acción rápida y supervivencia estilo *Arcade* (inspirado en el género *WarioWare*). El jugador debe enfrentarse a una sucesión de microdesafíos aleatorios con tiempo límite, acumulando puntos para liderar la clasificación global.

Este proyecto ha sido desarrollado como trabajo final con un fuerte enfoque en la **Arquitectura de Software** y el **Diseño de Interfaces (UX/UI)**.

---

## 🎮 Características Principales

* **Flujo Dinámico:** Transiciones rápidas y aleatorias entre niveles sin pantallas de carga intermedias.
* **Progresión de Dificultad:** El algoritmo gestor cambia dinámicamente el *pool* de minijuegos a versiones extremas (Nivel 2) una vez el jugador supera el umbral de victorias establecido.
* **12 Minijuegos Únicos:** Cada uno con mecánicas de interacción distintas diseñadas para evaluar reflejos, memoria, ritmo y precisión.
* **Generación Procedimental (PCG):** Los obstáculos y recompensas se instancian dinámicamente mediante algoritmos (ej. prevención de rachas injustas) para garantizar la rejugabilidad.
* **Persistencia en la Nube:** Sistema de *Ranking Global* conectado en tiempo real a **Firebase Realtime Database**.
* **Diseño Centrado en el Usuario (UX):** Interfaz desarrollada cumpliendo estrictamente las 10 Heurísticas de Usabilidad de Jakob Nielsen.

---

## 🛠️ Arquitectura y Tecnologías Destacadas

El código fuente (C#) está rigurosamente documentado mediante **XML (Javadoc style)** y aplica patrones de diseño avanzados:

* **Patrón Singleton:** Utilizado en los controladores de minijuegos (`Managers`) para un paso de mensajes global, seguro y eficiente.
* **Máquinas de Estados Finitos (FSM):** Implementadas en mecánicas temporales (ej. el minijuego del Semáforo) para transiciones lógicas sólidas y comportamientos impredecibles en niveles avanzados.
* **Corrutinas (Asincronía):** Gestión del *pacing* visual y bloqueos de interfaz mediante `IEnumerator` para evitar congelamientos del hilo principal.
* **Interacciones Nativas (UI):** Implementación de manipulaciones directas en el espacio físico como *Drag & Drop* (`OnMouseDrag`, `ScreenToWorldPoint`).
* **Optimización de Memoria (Garbage Collection):** Limpieza dinámica de objetos generados procedimentalmente al salir del *viewport* de la cámara (Culling manual).

---

## 🕹️ Catálogo de Minijuegos

1. **Minijuego Cesta:** Atrapa los objetos correctos moviéndote de lado a lado mientras esquivas trampas mortales.
2. **Minijuego Frutas:** Plataformas clásico. Recoge todas las frutas del escenario antes de que acabe el tiempo (incluye IA de persecución de la "Sombra" en dificultades altas).
3. **Minijuego Saltos:** *Side-Scroller* de supervivencia y reflejos a alta velocidad tipo *Geometry Dash*.
4. **Minijuego Machaca:** Rellena la barra de progreso a base de pura velocidad de pulsación mecánica.
5. **Minijuego Precisión:** Sincroniza tu impacto visual y táctil dentro de una franja milimétrica.
6. **Minijuego Meteoritos:** Supervivencia pura de esquiva de proyectiles en un espacio confinado.
7. **Minijuego Semáforo:** *Luz Roja, Luz Verde*. Sigilo y control de inercias físicas penalizadas por movimiento en estado rojo.
8. **Minijuego Memoria:** *Simón Dice*. Memoria visual secuencial con bloqueo preventivo de interfaz durante el turno de la CPU.
9. **Minijuego Gravedad:** *Runner* automático de supervivencia esquivando obstáculos mediante inversiones gravitacionales ("One-Button").
10. **Minijuego Lengua:** Mecánica de ratón direccional para cazar objetivos dinámicos estirando la lengua sin rozar las trampas.
11. **Minijuego Ordenador:** Evaluación de elementos y clasificación espacial rápida arrastrando objetos a sus contenedores lógicos.
12. **Minijuego Guitarra:** Minijuego de ritmo musical tipo *Guitar Hero* evaluando la coordinación y la rítmica del jugador.

---

## 📖 Documentación Adjunta

Como parte del despliegue del proyecto, se ha elaborado la siguiente documentación técnica:
* **Manual de Usuario:** Guía completa de navegación e interacciones (ubicada en la memoria del proyecto).
* **Análisis de Usabilidad:** Justificación de mecánicas basadas en estándares del HCI (Human-Computer Interaction).
* **Documentación de Código:** Todos los scripts principales cuentan con sumarios XML listos para ser extraídos por generadores como Doxygen o DocFX.

---

## ⚙️ Instrucciones de Montaje del Proyecto

Sigue estos pasos para compilar y ejecutar el proyecto en tu entorno local:

### Prerrequisitos Técnicos
* **Unity Hub** instalado.
* **Versión del Motor:** **Unity 6 (Versión 6000.3.2f1 LTS)** o superior.
* **Git** instalado en tu sistema.
* Conexión a Internet activa (necesaria para la conexión inicial con la base de datos de Firebase).

### Paso 1: Clonar el Repositorio

Abre tu terminal o consola de comandos y ejecuta:

git clone [https://github.com/francisgg256/MicroRush.git](https://github.com/francisgg256/MicroRush.git)

### Paso 2: Importar en Unity
1. Abre **Unity Hub** y haz clic en el botón **Add** (Añadir).
2. Navega por tu explorador de archivos y selecciona la carpeta `MicroRush` que acabas de clonar.
3. Unity importará las dependencias y reconstruirá la carpeta `Library`. *(Nota: Este proceso puede tardar unos minutos la primera vez que se abre el proyecto).*

### Paso 3: Ejecución
1. Ve a la ventana de **Project** dentro del editor de Unity y navega a la ruta: `Assets/MicroRush/Scene/`.
2. Abre la escena inicial llamada **`Inicio.unity`**.
3. Presiona el botón de **Play (▶️)** en la parte superior del editor para iniciar el juego desde el menú principal.
