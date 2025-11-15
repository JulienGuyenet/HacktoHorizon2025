"""
Script d'initialisation de la base de données
"""
import sqlite3
import os

DATABASE = 'inventory.db'

def init_db():
    """Initialiser la base de données avec le schéma"""
    # Supprimer la base de données existante si elle existe
    if os.path.exists(DATABASE):
        print(f"Base de données {DATABASE} existe déjà. Suppression...")
        os.remove(DATABASE)
    
    conn = sqlite3.connect(DATABASE)
    cursor = conn.cursor()
    
    cursor.execute('''
        CREATE TABLE IF NOT EXISTS inventory (
            id INTEGER PRIMARY KEY AUTOINCREMENT,
            reference TEXT NOT NULL,
            designation TEXT NOT NULL,
            famille TEXT,
            type TEXT,
            fournisseur TEXT,
            utilisateur TEXT,
            code_barre TEXT,
            numero_serie TEXT,
            informations TEXT,
            site TEXT,
            date_livraison TEXT,
            created_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP,
            updated_at TIMESTAMP DEFAULT CURRENT_TIMESTAMP
        )
    ''')
    
    conn.commit()
    print(f"Base de données {DATABASE} créée avec succès!")
    
    # Afficher le schéma
    cursor.execute("SELECT sql FROM sqlite_master WHERE type='table' AND name='inventory'")
    schema = cursor.fetchone()[0]
    print("\nSchéma de la table:")
    print(schema)
    
    conn.close()

if __name__ == '__main__':
    init_db()
