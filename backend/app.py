"""
Application Flask pour la gestion d'inventaire
"""
from flask import Flask, request, jsonify
from flask_cors import CORS
import sqlite3
import csv
import io
from datetime import datetime
import os

app = Flask(__name__)
CORS(app)

DATABASE = 'inventory.db'

def get_db():
    """Obtenir une connexion à la base de données"""
    conn = sqlite3.connect(DATABASE)
    conn.row_factory = sqlite3.Row
    return conn

def init_db():
    """Initialiser la base de données"""
    conn = get_db()
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
    conn.close()

@app.route('/api/health', methods=['GET'])
def health_check():
    """Endpoint de vérification de santé"""
    return jsonify({'status': 'healthy', 'message': 'API is running'})

@app.route('/api/items', methods=['GET'])
def get_items():
    """Récupérer tous les articles de l'inventaire"""
    try:
        conn = get_db()
        cursor = conn.cursor()
        
        # Paramètres de pagination
        page = request.args.get('page', 1, type=int)
        per_page = request.args.get('per_page', 50, type=int)
        offset = (page - 1) * per_page
        
        # Filtre de recherche
        search = request.args.get('search', '')
        
        if search:
            cursor.execute('''
                SELECT * FROM inventory 
                WHERE reference LIKE ? OR designation LIKE ? OR famille LIKE ?
                ORDER BY id DESC
                LIMIT ? OFFSET ?
            ''', (f'%{search}%', f'%{search}%', f'%{search}%', per_page, offset))
        else:
            cursor.execute('''
                SELECT * FROM inventory 
                ORDER BY id DESC
                LIMIT ? OFFSET ?
            ''', (per_page, offset))
        
        items = [dict(row) for row in cursor.fetchall()]
        
        # Compter le total
        cursor.execute('SELECT COUNT(*) as count FROM inventory')
        total = cursor.fetchone()['count']
        
        conn.close()
        
        return jsonify({
            'items': items,
            'total': total,
            'page': page,
            'per_page': per_page,
            'total_pages': (total + per_page - 1) // per_page
        })
    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/api/items/<int:item_id>', methods=['GET'])
def get_item(item_id):
    """Récupérer un article spécifique"""
    try:
        conn = get_db()
        cursor = conn.cursor()
        cursor.execute('SELECT * FROM inventory WHERE id = ?', (item_id,))
        item = cursor.fetchone()
        conn.close()
        
        if item:
            return jsonify(dict(item))
        else:
            return jsonify({'error': 'Item not found'}), 404
    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/api/items', methods=['POST'])
def create_item():
    """Créer un nouvel article"""
    try:
        data = request.get_json()
        
        required_fields = ['reference', 'designation']
        for field in required_fields:
            if field not in data:
                return jsonify({'error': f'Missing required field: {field}'}), 400
        
        conn = get_db()
        cursor = conn.cursor()
        
        cursor.execute('''
            INSERT INTO inventory (
                reference, designation, famille, type, fournisseur, 
                utilisateur, code_barre, numero_serie, informations, 
                site, date_livraison
            ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
        ''', (
            data.get('reference'),
            data.get('designation'),
            data.get('famille', ''),
            data.get('type', ''),
            data.get('fournisseur', ''),
            data.get('utilisateur', ''),
            data.get('code_barre', ''),
            data.get('numero_serie', ''),
            data.get('informations', ''),
            data.get('site', ''),
            data.get('date_livraison', '')
        ))
        
        item_id = cursor.lastrowid
        conn.commit()
        conn.close()
        
        return jsonify({'message': 'Item created successfully', 'id': item_id}), 201
    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/api/items/<int:item_id>', methods=['PUT'])
def update_item(item_id):
    """Mettre à jour un article existant"""
    try:
        data = request.get_json()
        
        conn = get_db()
        cursor = conn.cursor()
        
        # Vérifier si l'article existe
        cursor.execute('SELECT * FROM inventory WHERE id = ?', (item_id,))
        if not cursor.fetchone():
            conn.close()
            return jsonify({'error': 'Item not found'}), 404
        
        cursor.execute('''
            UPDATE inventory SET
                reference = ?,
                designation = ?,
                famille = ?,
                type = ?,
                fournisseur = ?,
                utilisateur = ?,
                code_barre = ?,
                numero_serie = ?,
                informations = ?,
                site = ?,
                date_livraison = ?,
                updated_at = CURRENT_TIMESTAMP
            WHERE id = ?
        ''', (
            data.get('reference'),
            data.get('designation'),
            data.get('famille', ''),
            data.get('type', ''),
            data.get('fournisseur', ''),
            data.get('utilisateur', ''),
            data.get('code_barre', ''),
            data.get('numero_serie', ''),
            data.get('informations', ''),
            data.get('site', ''),
            data.get('date_livraison', ''),
            item_id
        ))
        
        conn.commit()
        conn.close()
        
        return jsonify({'message': 'Item updated successfully'})
    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/api/items/<int:item_id>', methods=['DELETE'])
def delete_item(item_id):
    """Supprimer un article"""
    try:
        conn = get_db()
        cursor = conn.cursor()
        
        cursor.execute('SELECT * FROM inventory WHERE id = ?', (item_id,))
        if not cursor.fetchone():
            conn.close()
            return jsonify({'error': 'Item not found'}), 404
        
        cursor.execute('DELETE FROM inventory WHERE id = ?', (item_id,))
        conn.commit()
        conn.close()
        
        return jsonify({'message': 'Item deleted successfully'})
    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/api/import-csv', methods=['POST'])
def import_csv():
    """Importer des données depuis un fichier CSV"""
    try:
        if 'file' not in request.files:
            return jsonify({'error': 'No file provided'}), 400
        
        file = request.files['file']
        
        if file.filename == '':
            return jsonify({'error': 'No file selected'}), 400
        
        if not file.filename.endswith('.csv'):
            return jsonify({'error': 'File must be a CSV'}), 400
        
        # Lire le fichier CSV
        stream = io.StringIO(file.stream.read().decode("UTF-8"), newline=None)
        csv_reader = csv.DictReader(stream)
        
        conn = get_db()
        cursor = conn.cursor()
        
        imported_count = 0
        errors = []
        
        for idx, row in enumerate(csv_reader, start=2):  # start=2 car ligne 1 est l'en-tête
            try:
                # Mapper les colonnes CSV aux champs de la base de données
                cursor.execute('''
                    INSERT INTO inventory (
                        reference, designation, famille, type, fournisseur, 
                        utilisateur, code_barre, numero_serie, informations, 
                        site, date_livraison
                    ) VALUES (?, ?, ?, ?, ?, ?, ?, ?, ?, ?, ?)
                ''', (
                    row.get('Référence', ''),
                    row.get('Désignation', ''),
                    row.get('Famille', ''),
                    row.get('Type', ''),
                    row.get('Fournisseur', ''),
                    row.get('Utilisateur', ''),
                    row.get('Code barre', ''),
                    row.get('N° série', ''),
                    row.get('Informations', ''),
                    row.get('Site', ''),
                    row.get('Date de livraison', '')
                ))
                imported_count += 1
            except Exception as e:
                errors.append(f"Ligne {idx}: {str(e)}")
        
        conn.commit()
        conn.close()
        
        return jsonify({
            'message': f'{imported_count} items imported successfully',
            'imported_count': imported_count,
            'errors': errors
        })
    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/api/export-csv', methods=['GET'])
def export_csv():
    """Exporter l'inventaire en CSV"""
    try:
        conn = get_db()
        cursor = conn.cursor()
        cursor.execute('SELECT * FROM inventory ORDER BY id')
        items = cursor.fetchall()
        conn.close()
        
        # Créer le CSV
        output = io.StringIO()
        writer = csv.writer(output)
        
        # En-têtes
        writer.writerow([
            'Référence', 'Désignation', 'Famille', 'Type', 'Fournisseur',
            'Utilisateur', 'Code barre', 'N° série', 'Informations',
            'Site', 'Date de livraison'
        ])
        
        # Données
        for item in items:
            writer.writerow([
                item['reference'],
                item['designation'],
                item['famille'],
                item['type'],
                item['fournisseur'],
                item['utilisateur'],
                item['code_barre'],
                item['numero_serie'],
                item['informations'],
                item['site'],
                item['date_livraison']
            ])
        
        output.seek(0)
        return output.getvalue(), 200, {
            'Content-Type': 'text/csv',
            'Content-Disposition': 'attachment; filename=inventory_export.csv'
        }
    except Exception as e:
        return jsonify({'error': str(e)}), 500

@app.route('/api/stats', methods=['GET'])
def get_stats():
    """Obtenir des statistiques sur l'inventaire"""
    try:
        conn = get_db()
        cursor = conn.cursor()
        
        # Total d'articles
        cursor.execute('SELECT COUNT(*) as count FROM inventory')
        total_items = cursor.fetchone()['count']
        
        # Articles par famille
        cursor.execute('''
            SELECT famille, COUNT(*) as count 
            FROM inventory 
            WHERE famille != ''
            GROUP BY famille
        ''')
        by_famille = [dict(row) for row in cursor.fetchall()]
        
        # Articles par site
        cursor.execute('''
            SELECT site, COUNT(*) as count 
            FROM inventory 
            WHERE site != ''
            GROUP BY site
        ''')
        by_site = [dict(row) for row in cursor.fetchall()]
        
        conn.close()
        
        return jsonify({
            'total_items': total_items,
            'by_famille': by_famille,
            'by_site': by_site
        })
    except Exception as e:
        return jsonify({'error': str(e)}), 500

if __name__ == '__main__':
    # Initialiser la base de données au démarrage
    if not os.path.exists(DATABASE):
        init_db()
        print(f"Base de données {DATABASE} créée avec succès")
    
    app.run(debug=True, host='0.0.0.0', port=5000)
