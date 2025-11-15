# Import de données

Ce document décrit comment importer des données de meubles depuis des fichiers CSV ou Excel.

## 📂 Fichiers de données

Les fichiers de données d'exemple se trouvent dans le dossier `/docs/data` :
- `VIOTTE_Inventaire_20251114.csv` - Fichier CSV avec l'inventaire des meubles
- `VIOTTE_Inventaire_20251114.xlsx` - Version Excel du même inventaire

## 🔧 Format des fichiers

Les fichiers doivent contenir les colonnes suivantes (séparateur `;` pour CSV) :

| Colonne | Description | Obligatoire |
|---------|-------------|-------------|
| Référence | Référence unique du meuble | ✅ Oui |
| Désignation | Nom/description du meuble | ✅ Oui |
| Famille | Famille du meuble | Non |
| Type | Type de meuble | Non |
| Fournisseur | Fournisseur du meuble | Non |
| Utilisateur | Utilisateur actuel | Non |
| Code barre | Code barre | Non |
| N° série | Numéro de série | Non |
| Informations | Informations complémentaires | Non |
| Site | Localisation complète | Non |
| Date de livraison | Date de livraison | Non |

### Format du champ Site

Le champ `Site` suit le format : `{code}\{ville}\{type}\{bâtiment}\{étage}\{salle}`

Exemple : `25\BESANCON\Siege\VIOTTE\4eme etage\417`

Le service d'import parse automatiquement ce format pour créer des entités `Location` avec :
- **BuildingName** : Nom du bâtiment (ex: VIOTTE)
- **Floor** : Étage (ex: 4eme etage)
- **Room** : Salle/pièce (ex: 417)
- **Description** : Chemin complet

## 🚀 Utilisation de l'API

### Import depuis le fichier par défaut

Importe les données depuis le fichier CSV par défaut situé dans `/docs/data/VIOTTE_Inventaire_20251114.csv` :

```bash
POST /api/Import/default
```

**Exemple avec curl :**
```bash
curl -X POST http://localhost:5000/api/Import/default
```

**Réponse :**
```json
{
  "success": true,
  "importedCount": 4140,
  "message": "4140 meubles importés avec succès depuis le fichier par défaut"
}
```

### Import depuis un fichier uploadé

Importe des données depuis un fichier Excel (.xlsx, .xls) ou CSV (.csv) uploadé :

```bash
POST /api/Import/upload
Content-Type: multipart/form-data
```

**Exemple avec curl :**
```bash
curl -X POST http://localhost:5000/api/Import/upload \
  -F "file=@/chemin/vers/votre/fichier.csv"
```

**Réponse :**
```json
{
  "success": true,
  "importedCount": 100,
  "message": "100 meubles importés avec succès depuis fichier.csv"
}
```

### Validation d'un fichier

Valide la structure d'un fichier avant de l'importer :

```bash
POST /api/Import/validate
Content-Type: multipart/form-data
```

**Exemple avec curl :**
```bash
curl -X POST http://localhost:5000/api/Import/validate \
  -F "file=@/chemin/vers/votre/fichier.csv"
```

**Réponse :**
```json
{
  "isValid": true,
  "message": "Le fichier est valide et peut être importé"
}
```

## 💡 Conseils

1. **Validation préalable** : Utilisez l'endpoint `/validate` pour vérifier votre fichier avant l'import
2. **Format CSV** : Assurez-vous que votre CSV utilise le point-virgule (`;`) comme séparateur
3. **Encodage** : Utilisez UTF-8 pour éviter les problèmes d'encodage
4. **Données de test** : Commencez par tester avec le fichier par défaut via `/api/Import/default`

## 🔍 Gestion des erreurs

Le service d'import :
- Ignore les lignes vides
- Log les erreurs pour chaque ligne problématique sans arrêter l'import
- Continue l'import même si certaines lignes échouent
- Retourne le nombre total de meubles importés avec succès

## 📊 Résultats

L'import avec le fichier par défaut crée :
- **4140 meubles** avec toutes leurs propriétés
- **66 localisations uniques** extraites automatiquement des chemins `Site`

Chaque meuble est automatiquement lié à sa localisation correspondante dans la base de données.

## 🎯 Exemples de données importées

**Meuble importé :**
```json
{
  "reference": "FAUTDACTYOPE",
  "designation": "Fauteuil dactylo opérateur",
  "famille": "Mobilier de bureau",
  "type": "Fauteuil",
  "fournisseur": "EquipBuro",
  "codeBarre": "16953",
  "site": "25\\BESANCON\\Siege\\VIOTTE\\1er etage\\105",
  "locationId": 12
}
```

**Localisation créée automatiquement :**
```json
{
  "buildingName": "VIOTTE",
  "floor": "1er etage",
  "room": "105",
  "description": "25\\BESANCON\\Siege\\VIOTTE\\1er etage\\105"
}
```
