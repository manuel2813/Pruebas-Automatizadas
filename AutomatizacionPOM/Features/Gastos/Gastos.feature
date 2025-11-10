Feature: Módulo de Gastos - Registro y Navegación

Background: El usuario está en la consulta de gastos
    Given el usuario ingresa al ambiente "http://localhost:31096/"
    When el usuario inicia sesión con usuario "admin@plazafer.com" y contraseña "calidad"
    And accede al módulo "Gasto"
    And accede al submódulo "Ver Gasto"

@navegacion
Scenario: CP-GAS-006 - Navegacion exitosa al formulario de Nuevo Gasto
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@registro-exitoso
Scenario: CP-GAS-048 - Registrar un gasto simple solo con importe
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "2000"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "2000"

@validacion
Scenario: CP-GAS-011 - Validar inconsistencia con importe en cero
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "0"
    Then el sistema deberia mostrar el error de inconsistencia "Es necesario que el total sea mayor a 0."

@validacion
Scenario: CP-GAS-012 - Validar que el campo Total no acepta importes negativos
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "-100"
    Then el campo "Total" no deberia contener un valor negativo

@validacion
Scenario: CP-GAS-050 - Validar inconsistencia sin fecha
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "2000"
    And el usuario ingresa "" en el campo "Fecha"
    Then el sistema deberia mostrar el error de inconsistencia "Es necesario ingresar la fecha de gasto."

@validacion @registro
Scenario: CP-GAS-049 - Registrar gasto sin proveedor asigna "VARIOS"
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "3000"
    And el usuario ingresa "01/11/2025" en el campo "Fecha"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con proveedor "VARIOS"

@calculo
Scenario: CP-GAS-013 - Validar cálculo automático de Total (sin IGV)
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Importe" con "150.50"
    Then el campo "Total" deberia calcularse y mostrar "150.50"

@calculo @igv
Scenario: CP-GAS-014 - Validar cálculo automático de Total (con IGV)
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Importe" con "100"
    And el usuario marca la casilla "GASTO CON IGV"
    Then el campo "Total" deberia calcularse y mostrar "118.00"

@credito
Scenario: CP-GAS-047 - Registrar un gasto a "Crédito Rápido"
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "700"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "700"

@credito @modal
Scenario: CP-GAS-016 - Validar apertura del modal de Financiamiento
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "100"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"

#@anulacion
#Scenario: CP-GAS-004 y CP-GAS-028 - Buscar y validar apertura modal de Anulación
   # When el usuario busca el gasto "G-001-000001"
   # And el usuario hace clic en el boton "Anular" del primer gasto
    #Then el usuario deberia ver el modal de "ANULAR GASTO"

@edicion
Scenario: CP-GAS-026 - Buscar y Abrir un gasto para Ver/Editar
    When el usuario busca el gasto "NG02 - 31"
    And el usuario hace clic en el boton "Ver/Editar" del primer gasto
    Then el usuario deberia ver el modal de "Detalle de Gasto"
    When el usuario hace clic en el boton "Cerrar" del modal

@credito @exito
Scenario: CP_GAS_019_ValidarGeneracionDeCronogramaExitoso
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "100"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    
    # --- TU FLUJO MANUAL ---
    # 1. Pone la fecha
    When el usuario ingresa "04/11/2025" en el campo "Fecha" del financiamiento
    # 2. Clic en Refresh
    And el usuario hace clic en el boton "Generar Cronograma"
    # 3. Clic en Aceptar
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    # 4. Verifica que se cerró
    Then el usuario deberia ver el formulario de "Registro de Gasto"


@credito @exito
Scenario: CP_GAS_020_ValidarCronogramaMultiCuotaDiaEspecifico
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "100"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    
    # --- Tu nuevo flujo ---
    # 1. Cuota > 1
    When el usuario ingresa "2" en el campo "CUOTA(S)"
    # 2. Día de pago "28"
    And el usuario selecciona el dia "28"
    # 3. Clic en Refresh
    And el usuario hace clic en el boton "Generar Cronograma"
    # 4. Clic en Aceptar
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    # 5. Verifica que se cerró
    Then el usuario deberia ver el formulario de "Registro de Gasto"

# --- PRUEBAS CORREGIDAS ---

#@credito @registro
#Scenario: CP-GAS-019 - Registrar gasto con 1 cuota (sin inicial) y fecha de Hoy
  #  When el usuario hace clic en el boton "+ NUEVO GASTO"
  #  And el usuario llena el campo "Total" con importe "100"
   # And el usuario marca la casilla "AL CRÉDITO"
   # And el usuario selecciona la opción "Configurar"
  #  Then el usuario deberia ver el modal de "FINANCIAMIENTO"
  #  And el usuario ingresa "1" en el campo "CUOTA(S)"
   # And el usuario selecciona la "Fecha de Hoy"
   # And el usuario hace clic en el boton "ACEPTAR (del modal)"
  #  And el usuario hace clic en el boton "GUARDAR"
   # Then el gasto se registra y aparece en la grilla con importe "100"

@credito @registro
Scenario: CP-GAS-022 - Registrar gasto con 1 cuota (con inicial) y fecha de Hoy
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "100"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "40" en el campo "INICIAL"
    And el usuario ingresa "1" en el campo "CUOTA(S)"
    And el usuario genera el cronograma
    And el usuario selecciona la "Fecha de Hoy"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "100"

#@credito @registro
#Scenario: CP-GAS-019-Dia2 - Registrar gasto con 1 cuota (sin inicial) y fecha específica
   # When el usuario hace clic en el boton "+ NUEVO GASTO"
   # And el usuario llena el campo "Total" con importe "101"
    #And el usuario marca la casilla "AL CRÉDITO"
    #And el usuario selecciona la opción "Configurar"
   # Then el usuario deberia ver el modal de "FINANCIAMIENTO"
   # And el usuario ingresa "1" en el campo "CUOTA(S)"
   # And el usuario selecciona el dia "2"
    #And el usuario hace clic en el boton "ACEPTAR (del modal)"
    #And el usuario hace clic en el boton "GUARDAR"
   # Then el gasto se registra y aparece en la grilla con importe "101"

#@credito @registro
#Scenario: CP-GAS-022-Dia2 - Registrar gasto con 1 cuota (con inicial) y fecha específica
   # When el usuario hace clic en el boton "+ NUEVO GASTO"
    #And el usuario llena el campo "Total" con importe "102"
   # And el usuario marca la casilla "AL CRÉDITO"
    #And el usuario selecciona la opción "Configurar"
    #Then el usuario deberia ver el modal de "FINANCIAMIENTO"
   # When el usuario ingresa "50" en el campo "INICIAL"
   # And el usuario ingresa "1" en el campo "CUOTA(S)"
   # And el usuario selecciona el dia "2"
   # And el usuario hace clic en el boton "ACEPTAR (del modal)"
    #And el usuario hace clic en el boton "GUARDAR"
   # Then el gasto se registra y aparece en la grilla con importe "102"


# --- INICIO DE LOS 15 NUEVOS SCENARIOS DE CRÉDITO RÁPIDO ---










# --- FIN DE LOS 15 NUEVOS SCENARIOS ---


@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia1_C2_Imp101
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "101"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "2" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "1"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe1650
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "1650"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "1650"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia2_C3_Imp102
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "102"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "3" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "2"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe1990
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "1990"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "1990"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia3_C4_Imp103
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "103"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "4" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "3"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia4_C5_Imp104
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "104"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "5" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "4"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"





@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe555
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "555"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "555"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia5_C6_Imp105
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "105"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "6" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "5"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia6_C7_Imp106
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "106"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "7" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "6"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe1500
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "1500"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "1500"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia7_C8_Imp107
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "107"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "8" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "7"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia8_C9_Imp108
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "108"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "9" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "8"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"




@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe250
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "250"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "250"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia9_C10_Imp109
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "109"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "10" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "9"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"




@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe430
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "430"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "430"


@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia10_C11_Imp110
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "110"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "11" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "10"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia11_C12_Imp111
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "111"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "12" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "11"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia12_C13_Imp112
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "112"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "13" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "12"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"


@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe920
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "920"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "920"

@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe1130
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "1130"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "1130"



@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia13_C14_Imp113
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "113"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "14" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "13"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia14_C15_Imp114
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "114"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "15" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "14"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe800
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "800"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "800"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia15_C16_Imp115
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "115"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "16" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "15"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia16_C17_Imp116
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "116"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "17" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "16"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"


@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe1240
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "1240"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "1240"


@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia17_C18_Imp117
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "117"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "18" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "17"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia18_C19_Imp118
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "118"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "19" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "18"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"




@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe810
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "810"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "810"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia19_C20_Imp119
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "119"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "20" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "19"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia20_C21_Imp120
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "120"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "21" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "20"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"



@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe950
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "950"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "950"


@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia21_C22_Imp121
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "121"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "22" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "21"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia22_C23_Imp122
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "122"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "23" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "22"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia23_C24_Imp123
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "123"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "24" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "23"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe350
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "350"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "350"


@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia24_C25_Imp124
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "124"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "25" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "24"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia25_C26_Imp125
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "125"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "26" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "25"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia26_C27_Imp126
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "126"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "27" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "26"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"





@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe777
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "777"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "777"



@credito @credito_rapido
Scenario: CP_GAS_CredRapido_Importe625
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "625"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario hace clic en el boton "GUARDAR"
    Then el gasto se registra y aparece en la grilla con importe "625"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia27_C28_Imp127
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "127"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "28" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "27"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@credito @exito @regresion
Scenario: CP_GAS_Financ_Dia28_C29_Imp128
    When el usuario hace clic en el boton "+ NUEVO GASTO"
    And el usuario llena el campo "Total" con importe "128"
    And el usuario marca la casilla "AL CRÉDITO"
    And el usuario selecciona la opción "Configurar"
    Then el usuario deberia ver el modal de "FINANCIAMIENTO"
    When el usuario ingresa "29" en el campo "CUOTA(S)"
    And el usuario selecciona el dia "28"
    And el usuario hace clic en el boton "Generar Cronograma"
    And el usuario hace clic en el boton "ACEPTAR (del modal)"
    Then el usuario deberia ver el formulario de "Registro de Gasto"

@diagnostico
Scenario: Prueba de diagnóstico - Esto debe fallar
    When el usuario hace un paso de prueba tonto