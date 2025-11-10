Feature: LoginFeature

A short summary of the feature

@InicioSesion
Scenario: Inicio de sesion exitoso
    Given el usuario ingresa al ambiente 'https://taller2025-qa.sigesonline.com/'
    When el usuario inicia sesión con usuario 'admin@plazafer.com' y contraseña 'calidad'
