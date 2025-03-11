import * as http from 'http';
import * as httpProxy from 'http-proxy';

export default (on, config) => {
  // Créer un proxy pour les requêtes API
  const proxy = httpProxy.createProxyServer({
    target: 'http://localhost:8080',
    changeOrigin: true
  });
  
  // Créer un serveur qui redirigera les requêtes
  const server = http.createServer((req, res) => {
    // Ajouter les en-têtes CORS nécessaires
    res.setHeader('Access-Control-Allow-Origin', '*');
    res.setHeader('Access-Control-Allow-Methods', 'GET, POST, PUT, DELETE, OPTIONS');
    res.setHeader('Access-Control-Allow-Headers', 'Origin, X-Requested-With, Content-Type, Accept, Authorization');
    
    // Gérer les requêtes OPTIONS (pre-flight)
    if (req.method === 'OPTIONS') {
      res.writeHead(200);
      res.end();
      return;
    }
    
    // Transférer la requête au serveur API
    proxy.web(req, res, {}, (err) => {
      console.error('Erreur de proxy:', err);
      res.writeHead(500);
      res.end('Erreur de proxy');
    });
  });
  
  // Démarrer le serveur sur un port disponible
  const port = 3333; // Choisissez un port disponible
  server.listen(port);
  
  // Ajouter l'URL du proxy à la configuration de Cypress
  config.env.apiProxyUrl = `http://localhost:${port}`;
  
  return config;
};