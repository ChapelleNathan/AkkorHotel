describe('template spec', () => {
  it('Login', () => {
    cy.visit("/");
    cy.intercept('POST', 'http://localhost:8080/login', {
      statusCode: 200,
      body: {
          response: "tokentest"
        }
      }
    ).as('loginRequest');
    cy.contains("Connexion").click();
    let email = 'string@gmail.com';
    let password = 'Password123.';
    cy.get(".cy-email").type(email);
    cy.get(".cy-password").type(password);
    cy.get(".cy-button").click();
    cy.wait('@loginRequest');
    cy.contains("Accueil")
  })
})