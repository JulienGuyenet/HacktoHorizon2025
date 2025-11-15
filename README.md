# Application de Gestion d'Inventaire - Bourgogne Franche-Comté

Application web complète pour la gestion d'inventaire de meubles pour la région Bourgogne Franche-Comté.

## 📋 Description

Cette application permet de gérer un inventaire de meubles avec les fonctionnalités suivantes :
- Création, modification et suppression d'articles
- Import/Export de données via fichiers CSV
- Recherche et filtrage d'articles
- Statistiques sur l'inventaire
- API REST complète

## 🏗️ Architecture

L'application est composée de :
- **Backend** : API REST Flask avec base de données SQLite
- **Frontend** : Interface HTML (à intégrer depuis Canva)

## 🚀 Installation

### Prérequis

- Python 3.8 ou supérieur
- pip (gestionnaire de paquets Python)

### Installation du Backend

1. **Cloner le dépôt**
```bash
git clone https://github.com/JulienGuyenet/HacktoHorizon2025.git
cd HacktoHorizon2025
```

2. **Créer un environnement virtuel (recommandé)**
```bash
python -m venv venv

# Sur Windows
venv\Scripts\activate

# Sur Linux/Mac
source venv/bin/activate
```

3. **Installer les dépendances**
```bash
cd backend
pip install -r requirements.txt
```

4. **Initialiser la base de données**
```bash
python init_db.py
```

5. **Lancer le serveur**
```bash
python app.py
```

Le serveur démarre sur `http://localhost:5000`

## 📊 Format CSV pour l'Import

Le fichier CSV doit contenir les colonnes suivantes **dans cet ordre exact** :

| Colonne | Description | Obligatoire |
|---------|-------------|-------------|
| Référence | Référence unique de l'article | ✅ Oui |
| Désignation | Nom/Description de l'article | ✅ Oui |
| Famille | Catégorie de l'article (ex: Mobilier) | Non |
| Type | Type spécifique (ex: Bureau, Chaise) | Non |
| Fournisseur | Nom du fournisseur | Non |
| Utilisateur | Personne utilisant l'article | Non |
| Code barre | Code-barres de l'article | Non |
| N° série | Numéro de série | Non |
| Informations | Informations complémentaires | Non |
| Site | Localisation (ex: Dijon, Besançon) | Non |
| Date de livraison | Date de livraison (format libre) | Non |

### Exemple de fichier CSV

Un fichier exemple `example_inventory.csv` est fourni dans le dossier `backend/`.

```csv
Référence,Désignation,Famille,Type,Fournisseur,Utilisateur,Code barre,N° série,Informations,Site,Date de livraison
REF001,Bureau en chêne,Mobilier,Bureau,Ikea,Jean Dupont,1234567890123,SN001,Bureau ergonomique,Dijon,2024-01-15
REF002,Chaise de bureau,Mobilier,Chaise,Conforama,Marie Martin,1234567890124,SN002,Chaise réglable,Besançon,2024-01-20
```

### Étapes pour importer un CSV

#### Via l'API (curl)
```bash
curl -X POST -F "file=@votre_fichier.csv" http://localhost:5000/api/import-csv
```

#### Via l'interface Web
1. Aller sur la page d'import
2. Cliquer sur "Choisir un fichier"
3. Sélectionner votre fichier CSV
4. Cliquer sur "Importer"

## 🔌 API REST

### Endpoints disponibles

#### Vérification de santé
```
GET /api/health
```

#### Récupérer tous les articles
```
GET /api/items?page=1&per_page=50&search=chaise
```
Paramètres :
- `page` : Numéro de page (défaut: 1)
- `per_page` : Articles par page (défaut: 50, max: 100)
- `search` : Terme de recherche (optionnel)

#### Récupérer un article
```
GET /api/items/{id}
```

#### Créer un article
```
POST /api/items
Content-Type: application/json

{
  "reference": "REF001",
  "designation": "Bureau en chêne",
  "famille": "Mobilier",
  "type": "Bureau",
  "fournisseur": "Ikea",
  "utilisateur": "Jean Dupont",
  "code_barre": "1234567890123",
  "numero_serie": "SN001",
  "informations": "Bureau ergonomique avec tiroirs",
  "site": "Dijon",
  "date_livraison": "2024-01-15"
}
```

#### Mettre à jour un article
```
PUT /api/items/{id}
Content-Type: application/json

{
  "reference": "REF001",
  "designation": "Bureau en chêne modifié",
  ...
}
```

#### Supprimer un article
```
DELETE /api/items/{id}
```

#### Importer un CSV
```
POST /api/import-csv
Content-Type: multipart/form-data

file: [fichier CSV]
```

#### Exporter en CSV
```
GET /api/export-csv
```

#### Statistiques
```
GET /api/stats
```
Retourne :
- Nombre total d'articles
- Distribution par famille
- Distribution par site

## 🗃️ Structure de la Base de Données

### Table `inventory`

| Champ | Type | Description |
|-------|------|-------------|
| id | INTEGER | Identifiant unique (clé primaire) |
| reference | TEXT | Référence de l'article (obligatoire) |
| designation | TEXT | Désignation de l'article (obligatoire) |
| famille | TEXT | Famille de l'article |
| type | TEXT | Type de l'article |
| fournisseur | TEXT | Fournisseur |
| utilisateur | TEXT | Utilisateur |
| code_barre | TEXT | Code-barres |
| numero_serie | TEXT | Numéro de série |
| informations | TEXT | Informations complémentaires |
| site | TEXT | Site de localisation |
| date_livraison | TEXT | Date de livraison |
| created_at | TIMESTAMP | Date de création |
| updated_at | TIMESTAMP | Date de dernière modification |

## 🌐 Intégration Frontend

Pour intégrer votre HTML créé sur Canva :

1. Placer votre fichier `index.html` à la racine du projet
2. Modifier le code pour faire des appels à l'API :

```javascript
// Exemple : Récupérer tous les articles
fetch('http://localhost:5000/api/items')
  .then(response => response.json())
  .then(data => {
    console.log(data.items);
    // Afficher les données dans votre interface
  });

// Exemple : Importer un CSV
const formData = new FormData();
formData.append('file', fileInput.files[0]);

fetch('http://localhost:5000/api/import-csv', {
  method: 'POST',
  body: formData
})
  .then(response => response.json())
  .then(data => {
    console.log(data.message);
  });
```

## 🛠️ Développement

### Structure du projet
```
HacktoHorizon2025/
├── backend/
│   ├── app.py                 # Application Flask principale
│   ├── init_db.py             # Script d'initialisation de la BDD
│   ├── requirements.txt       # Dépendances Python
│   └── example_inventory.csv  # Exemple de fichier CSV
├── index.html                 # Frontend (à ajouter)
├── .gitignore
└── README.md
```

### Tests manuels

Pour tester l'API avec curl :

```bash
# Test de santé
curl http://localhost:5000/api/health

# Créer un article
curl -X POST http://localhost:5000/api/items \
  -H "Content-Type: application/json" \
  -d '{"reference":"TEST001","designation":"Article de test"}'

# Récupérer tous les articles
curl http://localhost:5000/api/items

# Importer le CSV d'exemple
curl -X POST -F "file=@backend/example_inventory.csv" http://localhost:5000/api/import-csv
```

## 🔒 Sécurité

- Les données sont stockées localement dans SQLite
- CORS est activé pour permettre les requêtes depuis le frontend
- En production, pensez à :
  - Désactiver le mode debug de Flask
  - Ajouter une authentification
  - Utiliser HTTPS
  - Valider et nettoyer toutes les entrées utilisateur

## 📝 Licence

Ce projet est développé pour la région Bourgogne Franche-Comté.

## 👥 Contribution

Pour contribuer au projet :
1. Fork le projet
2. Créer une branche (`git checkout -b feature/AmazingFeature`)
3. Commit les changements (`git commit -m 'Add some AmazingFeature'`)
4. Push vers la branche (`git push origin feature/AmazingFeature`)
5. Ouvrir une Pull Request

## 📞 Support

Pour toute question ou problème, ouvrir une issue sur GitHub.
