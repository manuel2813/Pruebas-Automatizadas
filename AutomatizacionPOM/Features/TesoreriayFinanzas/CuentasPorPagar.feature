Feature: Tesorería - Cuentas Por Cobrar/Pagar

Valida la navegación y las acciones principales del módulo de Cuentas por Cobrar y Pagar.

Background: El usuario está en la pantalla de Cuentas Por Cobrar/Pagar
    Given el usuario ingresa al ambiente "http://localhost:31096/"
    When el usuario inicia sesión con usuario "admin@plazafer.com" y contraseña "calidad"
    And accede al módulo "Tesorería y Finanzas"
    And accede al submódulo "Cuentas Por Cobrar/Pagar"

@tesoreria @CP-TES-002
Scenario: CP-TES-002 - Validar el cambio de vista a "Por Pagar"
    When el usuario hace clic en el radio button "Por Pagar"
    Then el boton "$ PAGAR CUOTA" deberia estar visible

@tesoreria @CP-TES-009
Scenario: CP-TES-009 - Validar apertura del modal de cobro con una cuota
    When el usuario selecciona la primera cuota
    And el usuario hace clic en el boton "$ COBRAR CUOTA"
    Then el usuario deberia ver el modal de "CUOTA POR COBRAR"
    # (El Then 'el usuario deberia ver el modal de...' ya lo tienes en 'Gastos.feature')
    # Si da error, podemos crear un paso [Then] duplicado en el Steps de Cuentas.

@tesoreria @CP-TES-008
Scenario: CP-TES-008 - Validar advertencia al cobrar cuotas de diferentes clientes
    When el usuario selecciona la primera cuota
    And el usuario selecciona la segunda cuota
    And el usuario hace clic en el boton "$ COBRAR CUOTA"
    Then el sistema muestra la advertencia "No se puede realizar el cobro, el cliente debe ser el mismo"