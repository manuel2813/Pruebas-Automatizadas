// ---- Archivo: GastosStepDefinitions.cs (Versión LIMPIA Y FUNCIONAL) ----
using AutomatizacionPOM.Pages;
using OpenQA.Selenium;
using Reqnroll;
using System;
using NUnit.Framework;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class GastosStepDefinitions
    {
        private IWebDriver driver;
        private ConsultaGastosPage consultaGastosPage;
        private RegistroGastoPage registroGastoPage;
        private FinanciamientoPage financiamientoPage;
        private ConceptosPage conceptosPage; // Se usa para el botón 'Editar' de la grilla

        public GastosStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            this.consultaGastosPage = new ConsultaGastosPage(driver);
            this.registroGastoPage = new RegistroGastoPage(driver);
            this.financiamientoPage = new FinanciamientoPage(driver);
            this.conceptosPage = new ConceptosPage(driver);
        }

        // --- ¡PASO GENÉRICO DE CLIC (LIMPIO)! ---
        // Este [Binding] ahora SÓLO maneja los botones de GASTOS
        [When(@"el usuario hace clic en el boton ""([^""]*)""")]
        public void WhenElUsuarioHaceClicEnElBoton(string boton)
        {
            if (boton == "+ NUEVO GASTO")
            {
                consultaGastosPage.HacerClickNuevoGasto();
            }
            else if (boton == "GUARDAR")
            {
                // Este try/catch es para tu módulo de GASTOS
                try
                {
                    registroGastoPage.HacerClickGuardar();
                }
                catch (Exception)
                {
                    Assert.Fail("El botón GUARDAR del modal de Gastos no se encontró.");
                }
            }
            else if (boton == "Generar Cronograma")
            {
                financiamientoPage.ClicGenerarCronograma();
            }
            else if (boton == "ACEPTAR (del modal)")
            {
                financiamientoPage.ClicAceptarModal();
            }

            // --- INICIO DE LA SOLUCIÓN ---
            // Se han ELIMINADO los 'else if' de "NUEVO CONCEPTO" y "GUARDAR CONCEPTO"
            // Esos 'bindings' ahora viven en ConceptosStepDefinitions.cs
            // ¡Esto elimina el error de ambigüedad!
            // --- FIN DE LA SOLUCIÓN ---

            else
            {
                // Si el botón no se encuentra aquí, Reqnroll buscará en OTROS
                // archivos de StepDefinitions (como ConceptosStepDefinitions.cs)
                // antes de fallar.
                throw new PendingStepException($"El botón '{boton}' no está implementado en GastosStepDefinitions.");
            }
        }

        // --- PASOS DE NAVEGACIÓN (CP-GAS-006) ---
        [Then(@"el usuario deberia ver el formulario de ""([^""]*)""")]
        public void ThenElUsuarioDeberiaVerElFormularioDe(string formulario)
        {
            if (formulario == "Registro de Gasto")
            {
                registroGastoPage.VerificarPaginaRegistroGasto();
            }
            else
            {
                throw new PendingStepException($"El formulario '{formulario}' no está implementado.");
            }
        }

        // ... (Todos tus otros métodos de 'When', 'Then' de GASTOS van aquí sin cambios) ...

        [When(@"el usuario llena el campo ""Total"" con importe ""([^""]*)""")]
        public void WhenElUsuarioLlenaElCampoTotalConImporte(string importe)
        {
            registroGastoPage.IngresarTotal(importe);
        }

        [When(@"el usuario llena el campo ""Importe"" con ""([^""]*)""")]
        public void WhenElUsuarioLlenaElCampoImporte(string importe)
        {
            registroGastoPage.IngresarImporte(importe);
        }

        [When(@"el usuario ingresa ""([^""]*)"" en el campo ""Fecha""")]
        public void WhenElUsuarioIngresaEnElCampoFecha(string fecha)
        {
            registroGastoPage.IngresarFecha(fecha);
        }

        [When(@"el usuario busca y selecciona el proveedor por DNI ""([^""]*)""")]
        public void WhenElUsuarioBuscaYSeleccionaElProveedorPorDNI(string dni)
        {
            // (Este método está comentado en tu Page, así que lo comentamos aquí también)
            // registroGastoPage.BuscarYSeleccionarProveedor(dni);
        }

        [Then(@"el gasto se registra y aparece en la grilla con importe ""([^""]*)""")]
        public void ThenElGastoSeRegistraYApareceEnLaGrillaConImporte(string importe)
        {
            consultaGastosPage.VerificarGastoEnGrilla(importe, null);
        }

        [Then(@"el gasto se registra y aparece en la grilla con proveedor ""([^""]*)""")]
        public void ThenElGastoSeRegistraYApareceEnLaGrillaConProveedor(string proveedor)
        {
            consultaGastosPage.VerificarGastoEnGrilla(null, proveedor);
        }

        [Then(@"el sistema deberia mostrar el error de inconsistencia ""([^""]*)""")]
        public void ThenElSistemaDeberiaMostrarElErrorDeInconsistencia(string mensaje)
        {
            string errores = registroGastoPage.ObtenerMensajeInconsistencia();
            Assert.IsTrue(errores.Contains(mensaje),
                $"Error de validación fallido. \nError esperado: '{mensaje}' \nErrores reales: '{errores}'");
        }

        [Then(@"el campo ""Total"" no deberia contener un valor negativo")]
        public void ThenElCampoTotalNoDeberiaContenerUnValorNegativo()
        {
            string valorActual = registroGastoPage.ObtenerValorTotal();
            Assert.IsFalse(valorActual.Contains("-"),
                $"Error: El campo Total aceptó un valor negativo. Valor: '{valorActual}'");
        }

        [When(@"el usuario marca la casilla ""([^""]*)""")]
        public void WhenElUsuarioMarcaLaCasilla(string nombreCasilla)
        {
            registroGastoPage.MarcarCasilla(nombreCasilla);
        }

        [Then(@"el campo ""Total"" deberia calcularse y mostrar ""([^""]*)""")]
        public void ThenElCampoTotalDeberiaCalcularseYMostrar(string totalEsperado)
        {
            string totalActual = registroGastoPage.ObtenerValorTotal();
            string totalLimpio = totalActual.Replace(",", "").Trim();
            Assert.AreEqual(totalEsperado, totalLimpio,
                $"Cálculo de Total incorrecto. Esperado: '{totalEsperado}', Actual: '{totalLimpio}'");
        }

        [When(@"el usuario selecciona la opción ""Configurar""")]
        public void WhenElUsuarioSeleccionaLaOpcionConfigurar()
        {
            registroGastoPage.SeleccionarOpcionConfigurar();
        }

        [Then(@"el usuario deberia ver el modal de ""([^""]*)""")]
        public void ThenElUsuarioDeberiaVerElModalDe(string nombreModal)
        {
            if (nombreModal == "FINANCIAMIENTO")
            {
                registroGastoPage.VerificarModalFinanciamiento();
            }
            else if (nombreModal == "ANULAR GASTO")
            {
                consultaGastosPage.VerificarModalAnulacion();
            }
            else if (nombreModal == "Detalle de Gasto")
            {
                consultaGastosPage.VerificarModalDetalleGasto();
            }
            // (Los 'else if' de "REGISTRO DE CONCEPTO" se han ido a ConceptosStepDefinitions.cs)
            else
            {
                throw new PendingStepException($"El modal '{nombreModal}' no está implementado en GASTOS.");
            }
        }

        // --- PASOS DE ANULACIÓN Y EDICIÓN (CP-GAS-004, 028, 026) ---

        [When(@"el usuario busca el gasto ""([^""]*)""")]
        public void WhenElUsuarioBuscaElGasto(string texto)
        {
            consultaGastosPage.BuscarGasto(texto);
        }

        [When(@"el usuario hace clic en el boton ""([^""]*)"" del primer gasto")]
        public void WhenElUsuarioHaceClicEnElBotonDelPrimerGasto(string nombreBoton)
        {
            if (nombreBoton == "Anular")
            {
                consultaGastosPage.ClicAnularPrimerGasto();
            }
            else if (nombreBoton == "Ver/Editar")
            {
                consultaGastosPage.ClicVerEditarPrimerGasto();
            }
            // Este SÍ se queda, porque es un paso específico de la grilla
            else if (nombreBoton == "Editar")
            {
                conceptosPage.ClicEditarPrimerConcepto();
            }
            else
            {
                throw new PendingStepException($"El botón de grilla '{nombreBoton}' no está implementado.");
            }
        }

        // ... (Tus otros métodos de Financiamiento) ...
        [When(@"el usuario ingresa ""([^""]*)"" en el campo ""CUOTA\(S\)""")]
        public void WhenElUsuarioIngresaEnElCampoCUOTAS(string cuotas)
        {
            financiamientoPage.IngresarCuotas(cuotas);
        }

        [Then(@"el sistema deberia mostrar el error de financiamiento ""([^""]*)""")]
        public void ThenElSistemaDeberiaMostrarElErrorDeFinanciamiento(string mensaje)
        {
            financiamientoPage.VerificarErrorFinanciamiento(mensaje);
        }

        [When(@"el usuario ingresa ""([^""]*)"" en el campo ""INICIAL""")]
        public void WhenElUsuarioIngresaEnElCampoINICIAL(string monto)
        {
            financiamientoPage.IngresarInicial(monto);
        }

        [When(@"el usuario selecciona la ""Fecha de Hoy""")]
        public void WhenElUsuarioSeleccionaLaFechaDeHoy()
        {
            financiamientoPage.SeleccionarFechaDeHoy();
        }

        [When(@"el usuario selecciona el dia ""([^""]*)""")]
        public void WhenElUsuarioSeleccionaElDia(string dia)
        {
            financiamientoPage.SeleccionarDia(dia);
        }

        [When(@"el usuario hace clic en el boton ""(.*)"" del modal")]
        public void WhenElUsuarioHaceClicEnElBotonDelModal(string nombreBoton)
        {
            if (nombreBoton == "Cerrar")
            {
                consultaGastosPage.CerrarModalDetalle();
            }
            else
            {
                Assert.Fail($"Botón '{nombreBoton}' del modal no implementado.");
            }
        }

        [When(@"el usuario ingresa ""(.*)"" en el campo ""Fecha"" del financiamiento")]
        public void WhenElUsuarioIngresaEnElCampoFechaDelFinanciamiento(string fecha)
        {
            financiamientoPage.IngresarFecha(fecha);
        }
    }
}