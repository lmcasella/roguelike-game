# 🗡️ Hacia las Profundidades

[![Jugar en itch.io](https://img.shields.io/badge/Jugar_en-itch.io-FA5C5C?style=for-the-badge&logo=itch.io)](https://kezgan.itch.io/hacia-las-profundidades)

Un videojuego de acción y exploración de mazmorras en 2D con mecánicas Roguelike, desarrollado en **Unity** como proyecto universitario. El jugador debe adentrarse en salas con enemigos generados dinámicamente, enfrentarse a oleadas de enemigos y gestionar sus recursos para sobrevivir el mayor tiempo posible.

## 🎮 Características Principales

* **Sistema de Salas:** Explorá 3 mazmorras interconectadas. Al entrar, las puertas se bloquean y deberás sobrevivir a oleadas de enemigos antes de poder avanzar.
* **Combate:**
  * Movimiento en 2D.
  * Sistema de ataque apuntado con el mouse.
  * Distintos tipos de habilidades para agregar variedad al combate: Ataque basico, habilidad Bola de Fuego, habilidad Miedo, habilidad Dash.
* **Interacción y Entorno:** Sistema de salud, gestión de maná, apertura de puertas y recolección de recompensas temporales en objetos del mapa.
* **Enemigos y Jefes:** Variedad de enemigos con distintos tipos de ataque (cuerpo a cuerpo y a distancia) y un enfrentamiento final contra un Jefe.
* **Progresión y Persistencia:** Conservá tus estadísticas entre niveles. Mejorá tu daño, aumentá tu cantidad de proyectiles o adquirí habilidades pasivas al finalizar cada sala.

## ⚙️ Arquitectura y Patrones de Diseño

El código fue estructurado priorizando la escalabilidad, el mantenimiento y los principios SOLID, cumpliendo con los requisitos técnicos del proyecto:

* **Arquitectura Basada en Eventos (Observer):** Desacoplamiento de sistemas mediante `GameEvents`. Las acciones importantes (daño, muerte de enemigos, cambios en la UI) se comunican a través de eventos, evitando dependencias.
* **Patrón Singleton:** Implementado para controladores globales que deben persistir durante todo el ciclo de vida del juego, específicamente el `GameManager` (estado del juego y persistencia de datos del jugador) y el `AudioManager`.
* **Sistemas Modulares:** La lógica del jugador y los enemigos está dividida en componentes específicos (`SystemHealth`, `PlayerMana`, `PlayerStats`), facilitando la reutilización de código.
* **Inteligencia Artificial (Steering Behaviors):** Enemigos controlados por IA básica utilizando comportamientos de dirección para perseguir al jugador o alejarse.

## 🕹️ Controles Básicos

* **Movimiento:** `W`, `A`, `S`, `D` / Flechas direccionales
* **Apuntar y Atacar:** Mouse + Clic Izquierdo
* **Dash (Evasión):** `Shift Izquierdo`
* **Pausa:** `Escape`

## 🚀 Cómo probar el proyecto en Unity

1. Cloná este repositorio: `git clone https://github.com/lmcasella/roguelike-game.git`
2. Abrí el proyecto utilizando **Unity Hub**.
3. Andá a la carpeta `Assets/Scenes` y abrí la escena `MainMenu`.
4. Play en el editor.
