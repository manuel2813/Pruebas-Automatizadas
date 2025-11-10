Feature: Módulo de Gastos - Reportes

Valida la generación y visualización de reportes de gastos.

Background: El usuario está en la pantalla de Reporte de Gasto
    Given el usuario ingresa al ambiente "http://localhost:31096/"
    When el usuario inicia sesión con usuario "admin@plazafer.com" y contraseña "calidad"
    And accede al módulo "Gasto"
    And accede al submódulo "Reporte de Gasto"

@reporte @camino-feliz
Scenario: CP-GAS-090 - Generar un reporte "Global"
    When el usuario ingresa el dia "1" en la Fecha Inicial del reporte
    And el usuario ingresa el dia "10" en la Fecha Final del reporte
    And el usuario selecciona el tipo de reporte "Global"
    And el usuario hace clic en el boton "VER"
    Then el visor de reportes deberia cargarse

@reporte @ui
Scenario: CP-GAS-085 - Validar visibilidad de campos por tipo "Establecimiento"
    When el usuario selecciona el tipo de reporte "Establecimiento"
    # Aquí podríamos añadir un Then para verificar que el campo "Establecimiento" aparece
    # (Necesitaríamos el XPath de ese campo)
    Then el visor de reportes no deberia ser visible aun

@reporte @viewer
Scenario: CP-GAS-098 - Validar la funcionalidad de Zoom del reporte
    When el usuario ingresa el dia "1" en la Fecha Inicial del reporte
    And el usuario ingresa el dia "10" en la Fecha Final del reporte
    And el usuario selecciona el tipo de reporte "Global"
    And el usuario hace clic en el boton "VER"
    Then el visor de reportes deberia cargarse
    When el usuario selecciona el zoom "150%"
    Then el zoom del reporte deberia ser "150%"