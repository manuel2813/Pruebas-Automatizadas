// ---- Archivo: ConceptosStepDefinitions.cs (Versión Final Corregida) ----
using AutomatizacionPOM.Pages;
using OpenQA.Selenium;
using Reqnroll;
using System;
using NUnit.Framework;

namespace AutomatizacionPOM.StepDefinitions
{
    [Binding]
    public class ConceptosStepDefinitions
    {
        private IWebDriver driver;
        private ConceptosPage conceptosPage;

        public ConceptosStepDefinitions(IWebDriver driver)
        {
            this.driver = driver;
            this.conceptosPage = new ConceptosPage(driver);
        }

        // --- PASOS DE VERIFICACIÓN ---
        [Then(@"el usuario deberia ver la consulta de ""([^""]*)""")]
        public void ThenElUsuarioDeberiaVerLaConsultaDe(string nombrePagina)
        {
            if (nombrePagina == "Conceptos")
            {
                conceptosPage.VerificarPaginaConceptos();
            }
            else
            {
                throw new PendingStepException($"La página '{nombrePagina}' no está implementada AQUÍ.");
            }
        }

        [Then(@"el concepto ""([^""]*)"" aparece en la grilla")]
        public void ThenElConceptoApareceEnLaGrilla(string nombreConcepto)
        {
            conceptosPage.VerificarConceptoEnGrilla(nombreConcepto);
        }

        [Then(@"el usuario deberia ver el modal de ""([^""]*)""")]
        public void ThenElUsuarioDeberiaVerElModalDe(string nombreModal)
        {
            if (nombreModal == "REGISTRAR CONCEPTO DE GASTO")
            {
                conceptosPage.VerificarModalRegistroVisible();
            }
            // (Tu escenario de edición CP-CON-006 usa este)
            else if (nombreModal == "REGISTRO DE CONCEPTO")
            {
                conceptosPage.VerificarModalRegistroVisible();
            }
            else
            {
                throw new PendingStepException($"El modal '{nombreModal}' no está implementado.");
            }
        }

        // --- PASOS DE ACCIÓN ---

        [When(@"el usuario escribe ""([^""]*)"" en el campo ""([^""]*)""")]
        public void WhenElUsuarioEscribeEnElCampo(string texto, string nombreCampo)
        {
            // Este binding ahora maneja los campos del MODAL
            if (nombreCampo == "FAMILIA")
            {
                conceptosPage.EscribirFamiliaEnModal(texto);
            }
            else if (nombreCampo == "SUFIJO")
            {
                conceptosPage.EscribirSufijoEnModal(texto);
            }
            // (Tu escenario de edición CP-CON-006 usa este)
            else if (nombreCampo == "Nombre")
            {
                // Si el campo 'Nombre' del modal de edición es diferente, necesitas
                // un nuevo locator y método en ConceptoPage.cs
                // Por ahora, asumimos que es el mismo que 'Familia'.
                conceptosPage.EscribirFamiliaEnModal(texto);
            }
            else
            {
                throw new PendingStepException($"El campo '{nombreCampo}' no está implementado.");
            }
        }

        // Binding para el combobox 'inline' que ya no usamos
        [When(@"el usuario escribe ""([^""]*)"" en el combobox de Concepto")]
        public void WhenElUsuarioEscribeEnElComboboxDeConcepto(string nombre)
        {
            // Este flujo está obsoleto, pero lo dejamos para no romper otras pruebas
            // conceptosPage.EscribirConcepto(nombre);
            throw new PendingStepException("Este paso usaba el formulario INLINE y ha sido reemplazado por el flujo MODAL.");
        }

        // Binding para el sufijo 'inline' que ya no usamos
        [When(@"el usuario escribe ""([^""]*)"" en el campo Sufijo")]
        public void WhenElUsuarioEscribeEnElCampoSufijo(string sufijo)
        {
            // Este flujo está obsoleto
            // conceptosPage.EscribirSufijo(sufijo);
            throw new PendingStepException("Este paso usaba el formulario INLINE y ha sido reemplazado por el flujo MODAL.");
        }

        [When(@"el usuario busca el concepto ""([^""]*)""")]
        public void WhenElUsuarioBuscaElConcepto(string nombreConcepto)
        {
            conceptosPage.BuscarConcepto(nombreConcepto);
        }

        // --- ¡BINDING DE BOTONES (LOCALIZADO)! ---
        // Este binding ahora maneja TODOS los botones de la feature 'Conceptos'
        [When(@"el usuario hace clic en el boton ""([^""]*)""")]
        public void WhenElUsuarioHaceClicEnElBoton(string nombreBoton)
        {
            if (nombreBoton == "NUEVO CONCEPTO")
            {
                conceptosPage.ClicNuevoConcepto();
            }
            else if (nombreBoton == "GUARDAR")
            {
                // Este es el botón GUARDAR del modal
                conceptosPage.ClicGuardarEnModal();
            }
            else
            {
                throw new PendingStepException($"El botón '{nombreBoton}' no está implementado en ConceptosStepDefinitions.");
            }
        }
    }
}