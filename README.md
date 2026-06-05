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
* **10 Minijuegos Únicos:** Cada uno con mecánicas de interacción distintas diseñadas para evaluar reflejos, memoria y precisión.
* **Generación Procedimental (PCG):** Los obstáculos y recompensas se instancian dinámicamente mediante algoritmos (ej. prevención de rachas injustas) para garantizar la rejugabilidad.
* **Persistencia en la Nube:** Sistema de *Ranking Global* conectado en tiempo real a **Firebase Realtime Database**.
* **Diseño Centrado en el Usuario (UX):** Interfaz desarrollada cumpliendo estrictamente las 10 Heurísticas de Usabilidad de Jakob Nielsen.

---

## 🛠️ Arquitectura y Tecnologías Destacadas

El código fuente (C#) está rigurosamente documentado mediante **XML (Javadoc style)** y aplica patrones de diseño avanzados:

* **Patrón Singleton:** Utilizado en los controladores de minijuegos (`Managers`) para un paso de mensajes global, seguro y eficiente.
* **Máquinas de Estados Finitos (FSM):** Implementadas en mecánicas temporales (ej. el minijuego del Semáforo) para transiciones lógicas sólidas.
* **Corrutinas (Asincronía):** Gestión del *pacing* visual y bloqueos de interfaz mediante `IEnumerator` para evitar congelamientos del hilo principal.
* **Interacciones Nativas (UI):** Implementación de manipulaciones directas en el espacio físico como *Drag & Drop* (`OnMouseDrag`, `ScreenToWorldPoint`).
* **Optimización de Memoria (Garbage Collection):** Limpieza dinámica de objetos generados procedimentalmente al salir del *viewport* de la cámara (Culling manual).

---

## 🕹️ Catálogo de Minijuegos

1. **Machaca-Botones:** Rellena la barra de progreso a base de velocidad de pulsación.
2. **Barra de Precisión:** Sincroniza el impacto dentro de una franja milimétrica.
3. **Collector 2D:** Plataformas clásico. Recoge todas las frutas antes del final de la cuenta atrás.
4. **Lluvia de Meteoritos:** Supervivencia de esquiva en espacio confinado.
5. **Dianas Point & Click:** Reflejos y precisión de ratón castigando los falsos positivos.
6. **Luz Roja, Luz Verde:** Sigilo y control de inercias físicas penalizadas por movimiento en estado rojo.
7. **Simón Dice:** Memoria visual con bloqueo preventivo de interfaz durante el turno de la CPU.
8. **Inversión Gravitacional:** Desplazamiento automático (Runner) esquivando obstáculos con mecánicas "One-Button".
9. **Clasificador (Drag & Drop):** Evaluación de elementos y clasificación espacial arrastrando objetos a sus contenedores.
10. **Salto de Sierras:** *Side-Scroller* de supervivencia vertical con físicas aplicadas.

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

### Paso 1: Clonar el Repositorio
bash 
git clone [https://github.com/francisgg256/MicroRush.git]

### Paso 2: Importar en Unity
1. Abre **Unity Hub** y haz clic en **Add** (Añadir).
2. Selecciona la carpeta `MicroRush` clonada.
3. Unity importará las dependencias y reconstruirá la carpeta `Library` (este proceso puede tardar unos minutos la primera vez).

### Paso 3: Ejecución
1. Ve a la ventana de **Project** en Unity y navega a: `Assets/MicroRush/Scene/`.
2. Abre la escena inicial llamada **`Inicio.unity`**.
3. Presiona el botón de **Play (▶️)** en el editor para iniciar el juego.
Presiona el botón de Play (▶️) en el editor para iniciar el juego.
