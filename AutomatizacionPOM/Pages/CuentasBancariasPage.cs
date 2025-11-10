using AutomatizacionPOM.Pages.Helpers;
using OpenQA.Selenium;
using NUnit.Framework;

namespace AutomatizacionPOM.Pages
{
    public class CuentasBancariasPage
    {
        private IWebDriver driver;
        private Utilities utilities;

        public CuentasBancariasPage(IWebDriver driver)
        {
            this.driver = driver;
            this.utilities = new Utilities(driver);
        }

        // --- Localizadores ---

        // Botón principal
        private By agregarCuentaButton = By.XPath("//button[@title='AGREGAR CUENTA BANCARIA']");

        // --- Localizadores de Fila (Dinámicos) ---
        // Usamos {row} para el número de fila (ej. '1' para la primera)
        // Corregí tus XPaths de Select2 para que sean estables

        private By TipoCuentaSelect(int row) =>
            By.XPath($"//tbody/tr[{row}]//span[contains(@id, 'select-tipo-cuenta-container')]");

        private By EntidadFinancieraSelect(int row) =>
            By.XPath($"//tbody/tr[{row}]//span[contains(@id, 'select-entidad-financiera-container')]");

        private By TitularInput(int row) =>
            By.XPath($"//tbody/tr[{row}]/td[4]/input[1]");

        private By MonedaSelect(int row) =>
            By.XPath($"//tbody/tr[{row}]//span[contains(@id, 'select-moneda-container')]");

        private By NumeroInput(int row) =>
            By.XPath($"//tbody/tr[{row}]/td[6]/input[1]");

        private By CciInput(int row) =>
            By.XPath($"//tbody/tr[{row}]/td[7]/input[1]");

        // Botones de acción de fila (Asumimos los 'title' basados en casos de prueba)
        private By EditarFilaButton(int row) =>
            By.XPath($"//tbody/tr[{row}]/td[11]//a[contains(@title, 'Editar')]");

        private By GuardarFilaButton(int row) =>
            By.XPath($"//tbody/tr[{row}]/td[11]//a[contains(@title, 'Guardar')]");

        // Verificación
        private By exitoAlert = By.XPath("//div[@class='sweet-alert showSweetAlert visible']//h2[text()='Correcto']");


        // --- Métodos de Acción ---

        public void ClickAgregarCuenta()
        {
            utilities.ClickButton(agregarCuentaButton);
            // Esperamos a que la nueva fila (tr[1]) aparezca y esté en modo edición
            utilities.WaitForElementToBeVisible(TitularInput(1));
        }

        public void ClickEditarFila(int row)
        {
            utilities.ClickButton(EditarFilaButton(row));
            // Esperamos a que la fila entre en modo edición
            utilities.WaitForElementToBeVisible(TitularInput(row));
        }

        public void IngresarTitular(int row, string titular)
        {
            utilities.ClearAndEnterText(TitularInput(row), titular);
        }

        public void SeleccionarTipoCuenta(int row, string tipo)
        {
            utilities.SelectOptionBySearch(TipoCuentaSelect(row), tipo);
        }

        public void SeleccionarEntidad(int row, string entidad)
        {
            utilities.SelectOptionBySearch(EntidadFinancieraSelect(row), entidad);
        }

        public void SeleccionarMoneda(int row, string moneda)
        {
            utilities.SelectOptionBySearch(MonedaSelect(row), moneda);
        }

        public void IngresarNumero(int row, string numero)
        {
            utilities.ClearAndEnterText(NumeroInput(row), numero);
        }

        public void IngresarCci(int row, string cci)
        {
            utilities.ClearAndEnterText(CciInput(row), cci);
        }

        public void ClickGuardarFila(int row)
        {
            utilities.ClickButton(GuardarFilaButton(row));
        }

        // --- Verificación ---

        public void VerificarGuardadoExitoso()
        {
            bool seMuestra = utilities.WaitForTextToBeVisible(exitoAlert, 10);
            Assert.IsTrue(seMuestra, "No apareció el mensaje 'Correcto' de guardado exitoso.");
        }
    }
}