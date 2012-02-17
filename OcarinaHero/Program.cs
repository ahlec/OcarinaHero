using System;

namespace OcarinaHero
{
    static class Program
    {
        static OcarinaHero game;
        static void Main(string[] args)
        {
            //try
            //{
                game = new OcarinaHero();
                game.Run();
            /*}
            catch (Exception e)
            {
                try
                {

                    OcarinaHeroLibrary.ApplicationSkin skin = (game.ApplicationSkin != null ?
                        game.ApplicationSkin : null);
                    OcarinaHeroLibrary.Screens.MessageWindow window = new OcarinaHeroLibrary.Screens.MessageWindow(skin,
                        e.ToString(), "Error: '" + e.Message + "'");
                    window.Run();
                    return;
                }
                catch (Exception furtherE)
                {
                    System.Windows.Forms.MessageBox.Show("Original Error: " + e.Message +
                        " (" + e.Data.ToString() + ")\n\nFurther Error: " + furtherE.Message, "Ocarina Hero - Error",
                               System.Windows.Forms.MessageBoxButtons.OK,
                               System.Windows.Forms.MessageBoxIcon.Error);
                }
            }*/
        }
    }
}

