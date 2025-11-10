Feature: Tesorería - Ingresos y Egresos

Valida los filtros y la navegación de la pantalla de Ingresos y Egresos.

Background: El usuario está en la pantalla de Ingresos y Egresos
    Given el usuario ingresa al ambiente "http://localhost:31096/"
    When el usuario inicia sesión con usuario "admin@plazafer.com" y contraseña "calidad"
    And accede al módulo "Tesorería y Finanzas"
    And accede al submódulo "Ingresos/Egresos"

@tesoreria @filtro @CP-TES-047
Scenario: CP-TES-047 - Validar el filtro de la grilla por "Cobros"
    When el usuario marca el check "Cobros"
    And el usuario desmarca el check "Pagos"
    And el usuario hace clic en el boton "Consultar"
    Then la grilla de ingresos/egresos se actualiza solo con "Cobros"

@tesoreria @filtro @CP-TES-048
Scenario: CP-TES-048 - Validar el filtro de la grilla por "Pagos"
    When el usuario desmarca el check "Cobros"
    And el usuario marca el check "Pagos"
    And el usuario hace clic en el boton "Consultar"
    Then la grilla de ingresos/egresos se actualiza solo con "Pagos"

@tesoreria @filtro @CP-TES-050
Scenario: CP-TES-050 - Validar el filtro de la grilla por "Pagador"
    When el usuario filtra por "Pagador" con "INVERSIONES TURISTICAS"
    Then la grilla de ingresos/egresos se actualiza con "INVERSIONES TURISTICAS"

@tesoreria @navegacion @CP-TES-053
Scenario: CP-TES-053 - Validar que el botón "$ INGRESO" abre el modal
    When el usuario hace clic en el boton "$ INGRESO"
    Then el usuario deberia ver el modal de "REGISTRO DE INGRESO VARIOS"
    # Este 'Then' reutiliza un paso que ya debes tener de tus pruebas de Gastos.