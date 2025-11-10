using OpenQA.Selenium;
using OpenQA.Selenium.Chrome;
using OpenQA.Selenium.Support.UI;
using SeleniumExtras.WaitHelpers;

namespace RegistroVenta
{
    public class RegistroVenta
    {
        private IWebDriver _driver;
        private WebDriverWait _wait;

        [SetUp]
        public void Setup()
        {
            _driver = new ChromeDriver();
            _wait = new WebDriverWait(_driver, TimeSpan.FromSeconds(30));
        }

        [Test]
        public void TestExampleTest()
        {
            // Navegar a la URL
            _driver.Navigate().GoToUrl("https://taller2025-qa.sigesonline.com/");

            // Encontrar los elementos del login
            var usernameField = _driver.FindElement(By.XPath("//input[@id='Email']"));
            var passwordField = _driver.FindElement(By.XPath("//input[@id='Password']"));
            var loginButton = _driver.FindElement(By.XPath("//button[normalize-space()='Iniciar']"));

            usernameField.SendKeys("admin@plazafer.com");
            passwordField.SendKeys("calidad");
            loginButton.Click();

            // Hacer clic en aceptar
            var acepptButton = _driver.FindElement(By.XPath("//button[normalize-space()='Aceptar']"));
            acepptButton.Click();

            Thread.Sleep(4000);
            // Comprobar que el login fue exitoso
            var succesElement = _wait.Until(drv => drv.FindElement(By.XPath("//img[@id='ImagenLogo']")));
            Assert.IsNotNull(succesElement, "No se encontró el elemento de éxito después del login.");

            //Ingresar al módulo Venta

            var saleButton = _driver.FindElement(By.XPath("//a[@class='menu-lista-cabecera']/span[text()='Venta']"));
            saleButton.Click();
            Thread.Sleep(4000);

            //  SELECCIONAR EL SUB-MÓDULO NUEVA VENTA
            var newSaleButton = _driver.FindElement(By.XPath("//a[normalize-space()='Nueva Venta']"));
            newSaleButton.Click();
            Thread.Sleep(20000);

            //Agregar un concepto
            try
            {
                var conceptSelection = By.XPath("/html/body/div[1]/div/section/div/div/div[1]/form/div[1]/div/div/registrador-detalles/div/div[1]/selector-concepto-comercial/ng-form/div/div[3]/div/div/span[1]/span[1]/span");
                _wait.Until(ExpectedConditions.ElementIsVisible(conceptSelection));
                IWebElement dropdown = _driver.FindElement(conceptSelection);
                dropdown.Click();
                var SelectODropdownOptions = By.CssSelector(".select2-results__options");
                _wait.Until(ExpectedConditions.ElementIsVisible(SelectODropdownOptions));
                IWebElement optionElement = _driver.FindElement(By.XPath($"//li[contains(text(), '{"400001474"}')]"));
                optionElement.Click();
                Thread.Sleep(2000);
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Error: No se encontró la opción '{"400001474"}' en el menú desplegable. Detalle: {ex.Message}");
            }

            //4. INGRESAR LA CANTIDAD "2" 
            var conceptAmount = By.Id("cantidad-0");
            var element = _wait.Until(ExpectedConditions.ElementToBeClickable(conceptAmount));
            element.SendKeys(Keys.Control + "a");
            element.SendKeys("2");
            Thread.Sleep(4000);

            //5. ACTIVAR IGV "SÍ"
            var igv = _driver.FindElement(By.Id("ventaigv0"));
            igv.Click();
            Thread.Sleep(4000);

            //6. INGRESAR CLIENTE "71310154"
            var client = By.Id("DocumentoIdentidad");
            var element2 = _driver.FindElement(client);
            element2.SendKeys(Keys.Control + "a");
            element2.SendKeys("71310154");
            element2.SendKeys(Keys.Enter);
            Thread.Sleep(4000);

            //7. SELECCIONAR TIPO DE COMPROBANTE "BOLETA"
            try
            {
                var voucher = By.XPath("/html/body/div[1]/div/section/div/div/div[1]/form/div[2]/facturacion-venta/form/div/div[2]/div/div[6]/selector-comprobante/div/ng-form/div[1]/div/span/span[1]/span");
                _wait.Until(ExpectedConditions.ElementIsVisible(voucher));
                IWebElement dropdown = _driver.FindElement(voucher);
                dropdown.Click();
                var SelectODropdownOptions = By.CssSelector(".select2-results__options");
                _wait.Until(ExpectedConditions.ElementIsVisible(SelectODropdownOptions));
                IWebElement optionElement = _driver.FindElement(By.XPath("//li[text()='BOLETA DE VENTA ELECTRONICA']"));
                optionElement.Click();
            }
            catch (NoSuchElementException ex)
            {
                Console.WriteLine($"Error: No se encontró la opción '{"BOLETA"}' en el menú desplegable. Detalle: {ex.Message}");
            }
            Thread.Sleep(4000);


            //8. SELECCIONAR TIPO DE PAGO "CONTADO"
            var cashPayment = By.CssSelector("label[for='radio1']");
            _wait.Until(ExpectedConditions.ElementToBeClickable(cashPayment));
            _driver.FindElement(cashPayment).Click();
            Thread.Sleep(4000);

            //9. SELECCIONAR MEDIO DE PAGO "TDEB"
            var debitCardButton = By.Id("labelMedioPago-0-18");
            _wait.Until(ExpectedConditions.ElementToBeClickable(debitCardButton));
            _driver.FindElement(debitCardButton).Click();
            Thread.Sleep(4000);

            //10. INGRESAR INFORMACIÓN DEL PAGO
            var information = _driver.FindElement(By.XPath("//div[@class='box box-primary box-solid']//textarea[@id='informacion']"));
            information.SendKeys("Cancelado");
            Thread.Sleep(4000);

            //11. GUARDAR VENTA
            var saveSale = _driver.FindElement(By.XPath("//button[normalize-space()='GUARDAR VENTA']"));
            saveSale.Click();
            Thread.Sleep(4000);

        }

        [TearDown]
        public void TearDown()
        {
            // Cerrar el navegador después de cada prueba
            _driver.Quit();
            _driver.Dispose(); // Libera memoria y recursos no administrados
        }
    }
}