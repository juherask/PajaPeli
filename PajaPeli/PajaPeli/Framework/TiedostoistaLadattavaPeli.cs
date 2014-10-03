using System;
using System.Collections.Generic;
using Jypeli;
using Jypeli.Assets;
using Jypeli.Controls;
using Jypeli.Effects;
using Jypeli.Widgets;
using System.IO;





abstract public class TiedostoistaLadattavaPeli : PhysicsGame
{
    public enum Tapahtuma
    {
        Iskee,
        Kuolee,
        Sattuu,
        Noukkii,
        Liikkuu,
        PeliLoppuu,
        Hyppaa,
        Voittaa,
        Tuntematon
    }

    //  Vaikoita joita ei kannata kokeilla muuttaa
    protected static int RUUDUN_KUVAN_LEVEYS = 32;
    protected static int RUUDUN_KUVAN_KORKEUS = 32;
    protected static int RUUDUN_LEVEYS = 64;
    protected static int RUUDUN_KORKEUS = 64;
    protected static int KARTAN_MAKSIMILEVEYS = 200;
    protected static int KARTAN_MAKSIMIKORKEUS = 30;
    protected static int TAUSTAN_MAKSIMILEVEYS = 1920;
    protected static int TAUSTAN_MAKSIMIKORKEUS = 1080;

    public static Color PELAAJAN_ALOITUSPAIKAN_VARI = Color.FromPaintDotNet(0, 14);
    public static Color KENTÄN_MAALIN_VARI = Color.FromPaintDotNet(1, 14);

    // Nämä pitävät sisällään peliin tiedostoista ladattavaa sisältöä.
    //  Dictionary tarkoittaa hakemistoa, jossa kuhunkin arvoon (esim. väri Color) on 
    //  linkitetty esim. lista kuvia (Image).
    public Dictionary<Image, string> Nimet = new Dictionary<Image, string>();
    public Dictionary<Color, List<Image>> HahmoKuvat;
    public Dictionary<Color, List<Image>> MaastoKuvat;
    public Dictionary<Color, List<Image>> EsineKuvat;
    public Dictionary<Tapahtuma, List<SoundEffect>> Tehosteet;
    public Dictionary<string, SoundEffect> Musiikki;
    public List<Image> Kartat;
    public List<Image> Taustakuvat;

    // Peliin valitut asiat
    public Image ValittuPelaajaHahmo;
    public Image ValittuKartta;
    public Image ValittuTausta;
    public SoundEffect ValittuMusiikki; 

    // Liikkumisesta kuuluva ääni
    SoundEffect liikkumisTehoste = null;

    // Nämä toteutetaan itse pelissä
    abstract public void KaynnistaPeli();
    abstract public void LisaaPelaajaPeliin();
    abstract public void LataaKentta();

    protected void LataaSisallotTiedostoista()
    {
        Apuri.AsetaPeli(this);

        // Ladataan peliin lisätty sisältö (taikuutta tapahtuu Apurit-luokassa)
        Apuri.LataaKuvatKansiosta(@"DynamicContent\Hahmot", RUUDUN_KUVAN_LEVEYS, RUUDUN_KUVAN_KORKEUS, ref Nimet, out HahmoKuvat);
        Apuri.LataaKuvatKansiosta(@"DynamicContent\Maasto", RUUDUN_KUVAN_LEVEYS, RUUDUN_KUVAN_KORKEUS, ref Nimet, out MaastoKuvat);
        Apuri.LataaKuvatKansiosta(@"DynamicContent\Esineet", RUUDUN_KUVAN_LEVEYS, RUUDUN_KUVAN_KORKEUS, ref Nimet, out EsineKuvat);
        Apuri.LataaKentatKansiosta(@"DynamicContent\Kartat", KARTAN_MAKSIMILEVEYS, KARTAN_MAKSIMIKORKEUS, ref Nimet, out Kartat);
        Apuri.LataaKentatKansiosta(@"DynamicContent\Taustat", TAUSTAN_MAKSIMILEVEYS, TAUSTAN_MAKSIMIKORKEUS, ref Nimet, out Taustakuvat);
    
        // TODO: Tee lataaja äänille ja lataa
        Apuri.LataaAanetKansiosta(@"DynamicContent\Tehosteet", out Tehosteet);
        Apuri.LataaAanetKansiosta(@"DynamicContent\Musiikki", out Musiikki);
    }

    /// <summary>
    /// Tätä kutsutaan kun pelaaja valitsee alkuvalikosta pelattavaksi satunnaisen pelin.
    /// </summary>
    public void SatunnainenPeliValittu()
    {
        // kuva,   
        ValittuPelaajaHahmo = null;
        if (HahmoKuvat.ContainsKey(PELAAJAN_ALOITUSPAIKAN_VARI))
        {
            ValittuPelaajaHahmo = RandomGen.SelectOne<Image>(HahmoKuvat[PELAAJAN_ALOITUSPAIKAN_VARI]);
        }
        LisaaPelaajaPeliin();

        ValittuKartta = RandomGen.SelectOne<Image>(Kartat);
        ValittuTausta = RandomGen.SelectOne<Image>(Taustakuvat);
        LataaKentta();

        // äänet, 
        SoitaSatunnainenTaustaMusiikki();

        // käy!
        Timer.SingleShot(0.1, KaynnistaPeli);
    }

    public void TiettyPeliValittu()
    {
        LisaaPelaajaPeliin();
        LataaKentta();
        if (ValittuMusiikki != null)
        {
            Sound biisi = ValittuMusiikki.CreateSound();
            biisi.IsLooped = true;
            biisi.Play();
        }
        Timer.SingleShot(0.1, KaynnistaPeli);
    }


    #region ÄäntenSoitto
    protected void ToistaTehoste(Tapahtuma tapahtuma)
    {
        if (Tehosteet.ContainsKey(tapahtuma))
        {
            bool liikkumisAaniLoppunut = liikkumisTehoste == null || !liikkumisTehoste.IsPlaying;
            if (tapahtuma != Tapahtuma.Liikkuu || liikkumisAaniLoppunut)
            {
                SoundEffect tehoste = RandomGen.SelectOne<SoundEffect>(Tehosteet[tapahtuma]);
                tehoste.Play(0.25, 0.0, 0.0);

                if (tapahtuma == Tapahtuma.Liikkuu)
                {
                    // Pistetään muistiin, että toistetaan liikkumisääntä
                    liikkumisTehoste = tehoste;
                }
            }
        }
    }
    protected void SoitaSatunnainenTaustaMusiikki()
    {
        if (Musiikki.Count == 0)
            return;
        SoundEffect biisi = RandomGen.SelectOne<SoundEffect>(new List<SoundEffect>(Musiikki.Values));
        biisi.Play();

        // Kun biisi loppuu, aloita uusi.
        Timer.SingleShot(biisi.Duration.Seconds + 2.0, SoitaSatunnainenTaustaMusiikki);
    }
    #endregion
}

