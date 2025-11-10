Feature: Módulo de Gastos - Administración de Conceptos

Background: El usuario está en la consulta de conceptos
    Given el usuario ingresa al ambiente "http://localhost:31096/"
    When el usuario inicia sesión con usuario "admin@plazafer.com" y contraseña "calidad"
    And accede al módulo "Gasto"
    And accede al submódulo "Concepto"

@navegacion
Scenario: CP-CON-001 - Navegacion exitosa a la consulta de Conceptos
    Then el usuario deberia ver la consulta de "Conceptos"

@creacion @manual
Scenario: CP-CON-002 - Crear un nuevo concepto (Ej: DESCUENTO)
    # --- FLUJO MODAL (Corregido con locators reales) ---
    
    # 1. Hacemos clic en el botón "NUEVO CONCEPTO" (el <a>)
    When el usuario hace clic en el boton "NUEVO CONCEPTO"
    
    # 2. Verificamos que el modal se abra
    Then el usuario deberia ver el modal de "REGISTRAR CONCEPTO DE GASTO"
    
    # 3. Escribimos en los campos DENTRO DEL MODAL
    When el usuario escribe "DESCUENTO" en el campo "FAMILIA"
    And el usuario escribe "DESC" en el campo "SUFIJO"
    
    # 4. Hacemos clic en el botón GUARDAR DENTRO DEL MODAL
    And el usuario hace clic en el boton "GUARDAR"
    
    # 5. El modal se cierra y el concepto aparece en la grilla
    Then el concepto "DESCUENTO" aparece en la grilla

@creacion @manual
Scenario: CP-CON-003 - Crear un nuevo concepto (Ej: PRUEBA QA)
    # --- FLUJO INLINE ---
    When el usuario escribe "PRUEBA QA" en el combobox de Concepto
    And el usuario escribe "QA" en el campo Sufijo
    And el usuario hace clic en el boton "GUARDAR CONCEPTO"
    Then el concepto "PRUEBA QA" aparece en la grilla
@creacion @basico
Scenario: CP-CON-005 - Ingresar un concepto básico (ej. ALQUILER)
    # --- CAMBIO CLAVE: Llamamos al botón de texto ---
    When el usuario hace clic en el boton "+ NUEVO CONCEPTO"
    Then el usuario deberia ver el modal de "INGRESO DE CONCEPTOS BASICOS"
    When el usuario selecciona el concepto basico "ALQUILER"   
    And el usuario hace clic en el boton "ACEPTAR (del modal basico)"
    Then el concepto "ALQUILER" aparece en la grilla

@edicion
Scenario: CP-CON-006 - Editar un concepto existente
    When el usuario busca el concepto "PRUEBA QA BIEN"
    And el usuario hace clic en el boton "Editar" del primer concepto
    Then el usuario deberia ver el modal de "REGISTRO DE CONCEPTO"
    When el usuario llena el campo "Nombre" con "PRUEBA QA BIEN (EDITADO)"   
    And el usuario hace clic en el boton "GUARDAR"
    Then el concepto "PRUEBA QA BIEN (EDITADO)" aparece en la grilla