using AutomatizacionPOM.Pages.Helpers;
using OpenQA.Selenium;
using NUnit.Framework;

namespace AutomatizacionPOM.Pages
{
    public class CuentasPorPagarPage
    {
        private IWebDriver driver;
        private Utilities utilities;

        public CuentasPorPagarPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilities = new Utilities(driver);
        }

        // --- Localizadores (Tus XPaths y Asumidos) ---

        // Radios (Tus XPaths)
        private By porCobrarRadio = By.XPath("//input[@id='radio1']");
        private By porPagarRadio = By.XPath("//input[@id='radio2']");

        // Botones (Asumidos de tu imagen 'image_167204.png')
        // *** SI FALLA, CORRIGE ESTOS XPATHS ***
        private By cobrarCuotaButton = By.XPath("//button[normalize-space()='$ COBRAR CUOTA']");
        private By pagarCuotaButton = By.XPath("//button[normalize-space()='$ PAGAR CUOTA']");

        // Grilla (Tus XPaths)
        private By buscarGeneralField = By.XPath("//div[@id='tabla-cuentas_filter']//input[@type='search']"); // XPath mejorado

        // Checkbox de la primera fila (Asumido)
        // *** SI FALLA, CORRIGE ESTE XPATH ***
        private By primeraFilaCheckbox = By.XPath("//table[@id='tabla-cuentas']/tbody/tr[1]/td[1]//input");
        private By segundaFilaCheckbox = By.XPath("//table[@id='tabla-cuentas']/tbody/tr[2]/td[1]//input");

        // Modal de Advertencia (Asumido de CP-TES-008)
        private By advertenciaModalText = By.XPath("//div[@class='sweet-alert showSweetAlert visible']//p");


        // --- Métodos de Acción ---

        public void ClickRadioPorPagar()
        {
            utilities.ClickButton(porPagarRadio);
            // Damos tiempo a que la grilla se recargue
            utilities.WaitForBlockOverlayToDisappear();
        }

        public void SeleccionarPrimeraCuota()
        {
            utilities.ClickButton(primeraFilaCheckbox);
        }

        public void SeleccionarSegundaCuota()
        {
            utilities.ClickButton(segundaFilaCheckbox);
        }

        public void ClickCobrarCuota()
        {
            utilities.ClickButton(cobrarCuotaButton);
        }

        // --- Métodos de Verificación (Asserts) ---

        public void VerificarBotonPagarEstaVisible()
        {
            // Verificamos que el botón $ PAGAR CUOTA esté visible
            bool estaVisible = utilities.WaitForTextToBeVisible(pagarCuotaButton, 5);
            Assert.IsTrue(estaVisible, "El botón '$ PAGAR CUOTA' no apareció después de cambiar a 'Por Pagar'.");
        }

        public void VerificarAdvertenciaDeClientesDiferentes()
        {
            // Verificamos el texto del caso CP-TES-008
            string textoEsperado = "No se puede realizar el cobro, el cliente debe ser el mismo";
            bool seMuestra = utilities.WaitForTextToContain(advertenciaModalText, textoEsperado, 5);
            Assert.IsTrue(seMuestra, $"No se mostró la advertencia '{textoEsperado}'.");
        }
    }
}