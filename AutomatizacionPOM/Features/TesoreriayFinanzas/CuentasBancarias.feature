Feature: Tesorería - Cuentas Bancarias

Valida la creación y edición "inline" de Cuentas Bancarias.

Background: El usuario está en la pantalla de Cuentas Bancarias
    Given el usuario ingresa al ambiente "http://localhost:31096/"
    When el usuario inicia sesión con usuario "admin@plazafer.com" y contraseña "calidad"
    And accede al módulo "Tesorería y Finanzas"
    And accede al submódulo "Cuentas Bancarias"

@tesoreria @creacion @CP-TES-092
Scenario: CP-TES-092 - Creación exitosa de una "CUENTA CORRIENTE" en "DOLAR"
    When el usuario hace clic en el boton "AGREGAR CUENTA BANCARIA"
    And el usuario selecciona "CUENTA CORRIENTE" como Tipo de Cuenta en la fila 1
    And el usuario selecciona "SCOTIABANK" como Entidad Financiera en la fila 1
    And el usuario ingresa "EMPRESA SAC PRUEBA" como Titular en la fila 1
    And el usuario selecciona "DOLAR AMERICANO" como Moneda en la fila 1
    And el usuario ingresa "9876543210" como Numero en la fila 1
    And el usuario ingresa "002987654321012345" como CCI en la fila 1
    And el usuario hace clic en Guardar de la fila 1
    Then la cuenta se guarda exitosamente

@tesoreria @edicion @CP-TES-083
Scenario: CP-TES-083 - Edición exitosa de un "TITULAR" de una cuenta existente
    # Asume que ya existe al menos una cuenta en la fila 1
    When el usuario hace clic en Editar de la fila 1
    And el usuario ingresa "TITULAR EDITADO PRUEBA" como Titular en la fila 1
    And el usuario hace clic en Guardar de la fila 1
    Then la cuenta se guarda exitosamente