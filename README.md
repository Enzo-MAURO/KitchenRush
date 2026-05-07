# KitchenRush – Jeu de gestion et cuisine inspiré d’Overcooked

## Présentation du projet

KitchenRush est un projet de jeu vidéo développé avec Unity inspiré de jeux de gestion et de coopération tels que *Overcooked* et *Cooking Fever*.

Le joueur doit préparer et servir des commandes dans différents restaurants à thèmes :
- Burger House
- Sushi Spot
- Pasta Perfection
- Pizza World

Le projet mélange :
- gestion du temps
- préparation de recettes
- interactions en temps réel
- gameplay arcade
- progression par niveaux

---

# Objectifs du projet

L’objectif principal du projet est de créer une expérience :
- dynamique
- accessible
- amusante
- rapide à prendre en main

Le joueur doit :
- récupérer des ingrédients
- préparer des recettes
- cuire certains aliments
- assembler les plats
- servir les commandes avant expiration du temps

---

# Technologies utilisées

## Moteur de jeu
- Unity 2022 LTS

## Langage
- C#

## Outils utilisés
- Unity Editor
- Git / GitHub
- Sketchfab
- Assets low poly stylisés
- Pixabay (audio)

---

# Structure du projet

## Organisation des dossiers

```text
Assets/
 ├── Scenes
 ├── Scripts
 ├── Prefabs
 ├── Materials
 ├── Textures
 ├── Models
 ├── Food
 ├── Audio
 └── UI
```

---

# Fonctionnalités développées

## 1. Système de commandes

Le joueur reçoit des commandes aléatoires comprenant :
- burgers
- boissons
- frites
- sushis
- makis

Les commandes sont affichées dans une interface dédiée.

Exemple :

```text
Commande :
- Sushi Saumon
- Maki Thon
- Eau
```

---

## 2. Système de préparation

### Burger House
- assemblage des ingrédients
- cuisson du steak
- préparation des frites
- gestion des boissons

### Sushi Spot
- cuisson du riz
- découpe du saumon et du thon
- préparation des makis
- assemblage des sushis

---

# Recettes implémentées

## Sushi saumon
```text
Riz cuit + saumon découpé
```

## Sushi thon
```text
Riz cuit + thon découpé
```

## Maki saumon
```text
Algue + riz cuit + saumon découpé
```

## Maki thon
```text
Algue + riz cuit + thon découpé
```

---

# Gameplay

## Déplacement du joueur
Le personnage se déplace en 3D avec :
- ZQSD
- Flèches directionnelles

Le personnage peut :
- interagir avec les objets
- porter des ingrédients
- déposer des éléments
- utiliser des stations

---

# Système d’interaction

Une touche d’interaction (`E`) permet :
- prendre un objet
- poser un objet
- lancer une cuisson
- découper un ingrédient
- interagir avec une station

---

# Interface utilisateur

## Menus réalisés
- écran principal
- sélection des mondes
- sélection des niveaux
- écran Game Over
- menu pause
- menu paramètres

---

# Menu paramètres

Le menu paramètres comprend :

## Audio
- volume principal
- volume musique
- volume effets sonores

## Affichage
- mode fenêtré
- plein écran fenêtre
- plein écran exclusif
- qualité graphique

## Jeu
- réinitialisation de la progression

---

# Direction artistique

Le projet utilise un style :
- low poly
- cartoon
- coloré
- inspiré d’Overcooked

Les assets utilisés proviennent :
- de créations personnelles
- de packs low poly
- d’assets Sketchfab
- d’assets Unity compatibles

---

# Ambiance sonore

Le projet utilise :
- musique de fond
- bruitages de cuisine
- sons de friteuse
- cuisson
- distributeur de boissons

---

# Architecture du gameplay

Le gameplay est construit autour :
- d’objets interactifs
- de stations de préparation
- de recettes
- d’un système de transformation d’ingrédients

Exemple :
```text
Saumon cru
→ découpe
→ saumon découpé
→ assemblage recette
```

---

# Difficultés rencontrées

## Techniques
- gestion des interactions
- import d’assets 3D
- compatibilité matériaux URP
- gestion des collisions
- mise en place du système d’inventaire

## Design
- création d’une cuisine lisible
- circulation du joueur
- équilibre visuel entre gameplay et décoration

---

# Améliorations futures

## Gameplay
- multijoueur local
- nouveaux restaurants
- nouvelles recettes
- clients animés
- système d’argent
- amélioration des commandes

## Technique
- animations avancées
- IA clients
- sauvegarde complète
- optimisation graphique

---

# Conclusion

KitchenRush est un projet de jeu de gestion et de cuisine mettant en pratique :
- le développement gameplay sous Unity
- la programmation en C#
- la gestion d’interactions 3D
- la création d’interfaces
- l’intégration d’assets et de systèmes audio

Le projet met particulièrement l’accent sur :
- la fluidité du gameplay
- la lisibilité
- l’ambiance visuelle
- l’expérience utilisateur

L’objectif final est de proposer un jeu fun, rapide et accessible inspiré des grands jeux de cuisine coopératifs modernes.
