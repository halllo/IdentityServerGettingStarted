export const environment = {
  production: true,
  backend_api: 'https://localhost:44385',

  oidc: {
    authority: 'https://localhost:44304/',
    clientId: 'frontend-angular',
    scope: 'openid profile email api1.read'
  }

};
