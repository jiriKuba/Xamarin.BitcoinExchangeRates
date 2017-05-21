# Xamarin.BitcoinExchangeRates
This app will get actual bitcoin exchange rates. Windows version of apps contains live tile project.

## Used APIs:
Bitcoin API: https://blockchain.info/ticker <br />
Bitcoin history API: http://api.coindesk.com/v1/bpi/historical/close.json?for=yesterday <br />
Exchange rates API: http://api.fixer.io/latest?base=USD

## Technologies:
Xamarin.Forms (Xaml), <br />
Xam.Plugins.Settings, <br />
MVVM Light, <br />
Json.NET

### TODOs:
- Add android splashscreen
- Replace transparent windows icon (add colors)
- Add globalization (czech language)
- Add true async menu loading

### Test
Tested on Windows 10 and Android devices (4.4, 6).

### Publish
Published on Windows Store: https://www.microsoft.com/store/apps/9pc70d2fwvp0 <br />
and Google Play: https://play.google.com/store/apps/details?id=com.kuba.bitcoinexchangerate

![alt text][logo]

[logo]: https://github.com/jiriKuba/Xamarin.BitcoinExchangeRates/blob/master/Bitcoin.Curses/Bitcoin.Curses.Droid/Resources/drawable-xxhdpi/icon.png "Logo"
