// See https://aka.ms/new-console-template for more information

using AssetTracking_davidnilsson;

Random roll = new Random();
User user = new User(); //will append all data on the user object so it can "travel"
Console.WriteLine("Welcome! Please enter your name:");
user.Name = CheckIfNullOrEmpty(user);
user = user.AutoFill(user);
MainMenu(user);

Console.WriteLine($"Thank you for playing, {user.Name}.");
Console.WriteLine("--program end");




//Base function, exit this and program ends
static void MainMenu(User user)
{
    while (true)
    {
        PrintMainMenu(user);
        int menuChoice = VerifyInt(user, 1, 4); //1-4 is the range of ints what will be passed through
        switch (menuChoice)
        {
            case 1:
                Create(user);
                continue;
            case 2:
                Read(user);
                continue;
            case 3:
                Delete(user);
                continue;
            case 4: //quit
                break;
            default:
                Console.WriteLine("something went wrong in MainMenu()");
                Console.ReadLine();
                continue;
        }
        break;
    }
}
static void PrintMainMenu(User user)
{
    Console.Clear();
    Console.WriteLine("-MAIN MENU-");
    Console.WriteLine();
    Console.WriteLine("1. Create new data");
    Console.WriteLine("2. Read data (print)");
    Console.WriteLine("3. Delete Data");
    Console.WriteLine("4. Quit program");
    Console.WriteLine();
}



//functions that verify inputs
static string CheckIfNullOrEmpty(User user)
{
    //only passes through input that is not empty when trimmed
    while (true)
    {
        Console.Write("> ");
        string input = Console.ReadLine().Trim();
        if (String.IsNullOrEmpty(input))
        {
            Console.WriteLine("Please input something");
            continue;
        }
        else { return input; }
    }
}
static int VerifyInt(User user, int lowRange, int highRange)
{
    //checking to see if input is not empty, then a number, then if its in the desired range
    while (true)
    {
        string input = CheckIfNullOrEmpty(user); //input not empty
        bool isInt = int.TryParse(input, out int number);
        if (!isInt) //input is an int
        {
            Console.WriteLine("Please input a number");
            continue;
        }
        else if (number < lowRange || number > highRange) //input is withing range
        {
            Console.WriteLine($"Please input a number between or equal to: {lowRange} and {highRange}");
            continue;
        }
        return number;
    }
}
static double VerifyDouble(User user)
{
    while (true)
    {
        //just checks to see if its a double and returns it. also, replacing , with . to be sure its not a type-o
        string input = CheckIfNullOrEmpty(user);
        bool isDouble = double.TryParse(input.Replace(",", "."), out double number);
        if (!isDouble)
        {
            Console.WriteLine("Please input a number");
        }
        return number;
    }
}
static DateTime VerifyDateTime(User user)
{
    Console.WriteLine("Input date when the asset was purchased (format DD-MM-YYYY):");
    while (true)
    {
        string input = CheckIfNullOrEmpty(user);
        //only allowing valid dates in the past
        bool isDate = DateTime.TryParse(input, out DateTime purchaseDate);
        if (!isDate)
        {
            Console.WriteLine("Please input a valid date");
            continue;
        }
        if ((purchaseDate - DateTime.Now).TotalDays > 0)
        {
            Console.WriteLine("Please input a date that has not occured yet");
            continue;
        }
        return purchaseDate;
    }
}



//create section
static void Create(User user)
{
    while (true)
    {
        PrintCreateMenu(user);
        int menuChoice = VerifyInt(user, 1, 3); //verifies input for menu
        switch (menuChoice)
        {
            case 1:
                CreateAsset(user);
                continue;
            case 2:
                CreateOffice(user);
                continue;
            case 3: //quit to MainMenu()
                break;
            default:
                Console.WriteLine("something went wrong in Create()");
                Console.ReadLine();
                continue;
        }
        break;
    }
}
static void PrintCreateMenu(User user)
{
    Console.Clear();
    Console.WriteLine("-CREATE DATA-");
    Console.WriteLine();
    Console.WriteLine("1. Create new asset");
    Console.WriteLine("2. Create new office");
    Console.WriteLine("3. Go back to main menu");
    Console.WriteLine();
    Console.WriteLine("What type of data do you want to create:");
}
static void CreateAsset(User user)
{
    while (true)
    {
        Random roll = new Random();
        PrintOffices(user, "CREATE ASSET");
        if (user.Offices.Count() == 0) { break; }
        Console.WriteLine("Choose what office this asset should be registered in:");

        //selects the office (and currency) for the new asset
        int officeChoice = VerifyInt(user, 1, user.Offices.Count());
        user.ChosenOffice = user.Offices[officeChoice - 1];
        user.ChosenCurrency = user.Offices[officeChoice - 1].Currency;

        PrintAssetClasses(user);
        //selects what type of asset should be created
        int assetChoice = VerifyInt(user, 1, user.AssetClasses.Count() + 1); //+1 because we give option to create a new assetclass
        //if user want to create a new type of asset, then so be it
        if (assetChoice == user.AssetClasses.Count() + 1) { CreateAssetClass(user); }
        user.ChosenAssetClass = user.AssetClasses[assetChoice - 1];

        //choosing between a random brand or own input of brand
        Console.WriteLine("Input the brand of your new asset or input 'R' for a random brand:");
        string brand = CheckIfNullOrEmpty(user);
        if (brand.ToLower() == "r") { brand = user.Brands[roll.Next(0, user.Brands.Length + 1)]; }

        //choosing between a random model or own input of model
        Console.WriteLine("Input the model of you new asset or input 'R' for a random model:");
        string model = CheckIfNullOrEmpty(user);
        if (model.ToLower() == "r") { model = user.RandomModelName(user); }

        //asking for price
        Console.WriteLine($"Input the local price in the currency '{user.Offices[officeChoice - 1].Currency}':");
        double localPrice = VerifyDouble(user);

        //asking for purchase date
        DateTime purchaseDate = VerifyDateTime(user);

        //creating the new asset
        Asset newAsset = new Asset
            (
            model,
            brand,
            user.ChosenAssetClass,
            purchaseDate,
            user.ChosenOffice,
            user.ChosenCurrency
            );
        newAsset.LocalPrice = localPrice;
        newAsset.DollarPrice = newAsset.CalcDollarPrice(user, localPrice, user.ChosenCurrency);

        user.Offices[officeChoice - 1].Assets.Add(newAsset);
        Console.WriteLine($"The new {user.ChosenAssetClass}-asset was successfully created and placed in '{user.ChosenOffice.Name}'");
        Console.Write("(press enter to continue) ");
        Console.ReadLine();
        break;
    }
}
static void CreateAssetClass(User user)
{
    Console.WriteLine("What should the new type of asset be called (eg. clothes, keys, idbadges):");
    string newAssetClass = CheckIfNullOrEmpty(user);

    //adds the new type of asset into the array of assetclasses
    Array.Resize(ref user.AssetClasses, user.AssetClasses.Length + 1);
    user.AssetClasses[user.AssetClasses.Length -1] = newAssetClass;
}
static void CreateOffice(User user)
{
    //prints offices and asks user for new city
    if (user.Offices.Count() != 0) { PrintOffices(user, "CREATE OFFICE"); }
    else 
    { 
        Console.Clear();
        Console.WriteLine("-CREATE OFFICE-");
        Console.WriteLine();
    }
    Console.WriteLine("Input city for your new office:");
    string city = CheckIfNullOrEmpty(user);

    //asks user to choose currency for the new office
    PrintCurrencies(user, city);
    int currencyChoice = VerifyInt(user, 1, user.Currencies.Length + 1);
    //if they want to create a new currency so be it
    if (currencyChoice == user.Currencies.Length + 1) { CreateCurrency(user); }

    Office newOffice = new Office(city, user.Currencies[currencyChoice -1]);
    user.Offices.Add(newOffice);

    Console.WriteLine($"The new office in '{city}' was successfully created. local currency: {user.Currencies[currencyChoice -1]}");
    Console.Write("(press enter to continue) ");
    Console.ReadLine();
}
static void CreateCurrency(User user)
{
    Console.WriteLine("What is the abbreviation of your new currency:");
    string currencyName = CheckIfNullOrEmpty(user);

    Console.WriteLine("How many of this currency do you need to have 1 dollar (USD):");
    double rateFromDollar = VerifyDouble(user);

    //adds the currency name and rate with matching indexes
    Array.Resize(ref user.Currencies, user.Currencies.Length + 1);
    user.Currencies[user.Currencies.Length - 1] = currencyName;

    Array.Resize(ref user.CurrencyRatesFromDollar, user.CurrencyRatesFromDollar.Length + 1);
    user.CurrencyRatesFromDollar[user.CurrencyRatesFromDollar.Length - 1] = rateFromDollar;
}



//delete section
static void Delete(User user)
{
    while (true)
    {
        PrintDeleteMenu(user);
        int menuChoice = VerifyInt(user, 1, 3); //verifies input for menu
        switch (menuChoice)
        {
            case 1:
                DeleteAsset(user);
                continue;
            case 2:
                DeleteOffice(user);
                continue;
            case 3: //quit to MainMenu()
                break;
            default:
                Console.WriteLine("something went wrong in Delete()");
                Console.ReadLine();
                continue;
        }
        break;
    }
}
static void PrintDeleteMenu(User user)
{
    Console.Clear();
    Console.WriteLine("-DELETE DATA-");
    Console.WriteLine();
    Console.WriteLine("1. Delete an asset");
    Console.WriteLine("2. Delete an office");
    Console.WriteLine("3. Go back to main menu");
    Console.WriteLine();
    Console.WriteLine("What type of data do you want to delete:");
}
static void DeleteAsset(User user)
{
    while (true)
    {
        //asks from what office user wants to delete assets from
        PrintOffices(user, "DELETE ASSET");
        if (user.Offices.Count() == 0) { break; } //go back if there are no offices
        Console.WriteLine("Choose what office you want to delete assets from:");
        int officeChoice = VerifyInt(user, 1, user.Offices.Count());
        user.ChosenOffice = user.Offices[officeChoice - 1];

        PrintAssetsFromOffice(user);
        if (user.ChosenOffice.Assets.Count() == 0) { break; } //go back if no products in city
        int deleteChoice = VerifyInt(user, 1, user.ChosenOffice.Assets.Count() + 1);
        //if user chose last option in menu delete all assets, otherwise the asset user chose
        if (deleteChoice == user.ChosenOffice.Assets.Count() + 1) { DeleteAllAssets(user); }
        else { DeleteThisAsset(user, deleteChoice); }
        break;
    }
}
static void DeleteThisAsset(User user, int deleteChoice)
{
    string assetClass = user.ChosenOffice.Assets[deleteChoice - 1].AssetClass;
    string brand = user.ChosenOffice.Assets[deleteChoice - 1].Brand;
    string model = user.ChosenOffice.Assets[deleteChoice - 1].ModelName;

    user.ChosenOffice.Assets.Remove(user.ChosenOffice.Assets[deleteChoice - 1]);

    Console.WriteLine($"{assetClass}, {brand}, {model} was successfully deleted from {user.ChosenOffice.Name}");
    Console.Write("(press enter to continue) ");
    Console.ReadLine();
}
static void DeleteAllAssets(User user)
{
    int counter = 0;
    foreach (Asset asset in user.ChosenOffice.Assets)
    {
        Console.WriteLine($"Deleting: {asset.AssetClass}, {asset.Brand} {asset.ModelName}");
        counter++;
    }
    user.ChosenOffice.Assets.RemoveAll(x => x.Office == user.ChosenOffice);
    Console.WriteLine();
    Console.WriteLine($"Successfully removed all ({counter}) products from {user.ChosenOffice.Name}");
    Console.Write("press enter to continue) ");
    Console.ReadLine();
}
static void DeleteOffice(User user)
{
    while (true)
    {
        PrintOffices(user, "DELETE OFFICE");
        if (user.Offices.Count() == 0) { break; } //go back if there are no offices
        Console.WriteLine("Choose what office you want to delete: ");
        int deleteChoice = VerifyInt(user, 1, user.Offices.Count());

        string deleted = user.Offices[deleteChoice - 1].Name;
        user.Offices.RemoveAll(x => x.Name == deleted);
        Console.WriteLine($"The office in '{deleted}' was successfully deleted");
        Console.Write("(press enter to continue) ");
        Console.ReadLine();
        break;
    }
}



//read section
static void Read(User user)
{
    List<Asset> allAssets = new List<Asset>();
    foreach (Office office in user.Offices)
    {
        foreach (Asset asset in office.Assets)
        {
            allAssets.Add(asset);
        }
    }
    allAssets.OrderBy(x => x.Office.Name).ThenBy(x => x.DollarPrice);

    Console.Clear();
    Console.WriteLine
        (
        "Type".PadRight(15) +
        "Brand".PadRight(15) +
        "Model".PadRight(15) +
        "Office".PadRight(15) +
        "Purchase date".PadRight(15) +
        "USD price".PadRight(15) +
        "Currency".PadRight(15) +
        "Local price".PadRight(15)
        );
    Console.WriteLine
        (
        "----".PadRight(15) +
        "-----".PadRight(15) +
        "-----".PadRight(15) +
        "------".PadRight(15) +
        "-------------".PadRight(15) +
        "---------".PadRight(15) +
        "--------".PadRight(15) +
        "-----------".PadRight(15)
        );

    //if ((asset.PurchaseDate - DateTime.Today).TotalDays <= (365 * 2.75 *-1)) { Console.ForegroundColor = ConsoleColor.Red; } 
    //else if ((asset.PurchaseDate - DateTime.Today).TotalDays <= (365 * 2.5 *-1)) { Console.ForegroundColor = ConsoleColor.Yellow; } 

    TimeSpan threeMonthsAway = DateTime.Today - DateTime.Today.AddMonths(-33);
    TimeSpan sixMonthsAway = DateTime.Today - DateTime.Today.AddMonths(-30);

    foreach (Asset asset in allAssets)
    {
        TimeSpan assetAge = DateTime.Today - asset.PurchaseDate;
        if (assetAge >= threeMonthsAway) { Console.ForegroundColor = ConsoleColor.Red; }
        else if (assetAge >= sixMonthsAway) { Console.ForegroundColor = ConsoleColor.Yellow; }
        
        Console.WriteLine
            (
            $"{asset.AssetClass}".PadRight(15) +
            $"{asset.Brand}".PadRight(15) +
            $"{asset.ModelName}".PadRight(15) +
            $"{asset.Office.Name}".PadRight(15) +
            $"{asset.PurchaseDate.ToString("dd/MM/yyyy")}".PadRight(15) +
            $"{asset.DollarPrice.ToString("N2")}".PadRight(15) +
            $"{asset.Currency}".PadRight(15) +
            $"{asset.LocalPrice.ToString("N2")}".PadRight(15)
            );
        Console.ResetColor();
    }
    Console.WriteLine();
    Console.WriteLine("Press enter to go back to main menu");
    Console.ReadLine();
}
static void PrintOffices(User user, string doWhat)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine($"-{doWhat}-");
        Console.WriteLine();
        //no offices? print this and go back
        if (user.Offices.Count() == 0)
        {
            Console.WriteLine("There are no offices registrered. Please create one");
            Console.WriteLine();
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
            break;
        }
        Console.WriteLine("Current offices:");
        for (int i = 0; i < user.Offices.Count(); i++)
        {
            Console.WriteLine($"{i + 1}. {user.Offices[i].Name}");
        }
        Console.WriteLine();
        break;
    }
}
static void PrintAssetClasses(User user)
{
    Console.Clear();
    Console.WriteLine("-CREATE ASSET-");
    Console.WriteLine();
    Console.WriteLine("Current types of assets:");
    for (int i = 0; i < user.AssetClasses.Length; i++)
    {
        Console.WriteLine($"{i + 1}. {user.AssetClasses[i]}");
    }
    Console.WriteLine($"{user.AssetClasses.Length +1}. to create a new type of asset for this new product");
    Console.WriteLine();
    Console.WriteLine("Choose what type of asset this new product should be registered in:");
}
static void PrintCurrencies(User user, string city)
{
    Console.Clear();
    Console.WriteLine($"-CREATE OFFICE-");
    Console.WriteLine();
    Console.WriteLine($"Chosen city: {city}");
    Console.WriteLine();
    Console.WriteLine("Currencies:");
    for (int i = 0; i < user.Currencies.Count(); i++)
    {
        Console.WriteLine($"{i + 1}. {user.Currencies[i]}");
    }
    Console.WriteLine($"{user.Currencies.Length + 1}. to create a new currency for this new product");
    Console.WriteLine();
    Console.WriteLine("Choose what currency this new office should have as local currency:");
}
static void PrintAssetsFromOffice(User user)
{
    while (true)
    {
        Console.Clear();
        Console.WriteLine("-DELETE ASSET-");
        Console.WriteLine();
        //no assets in city? print this and go back
        if (user.ChosenOffice.Assets.Count() == 0)
        {
            Console.WriteLine("no assets added to this office yet");
            Console.WriteLine();
            Console.Write("(press enter to continue) ");
            Console.ReadLine();
            break;
        }
        //if assets are registerred, then choose:
        Console.WriteLine($"Current assets in {user.ChosenOffice.Name}:");
        for (int i = 0; i < user.ChosenOffice.Assets.Count(); i++)
        {
            Console.WriteLine($"{i + 1}. {user.ChosenOffice.Assets[i].AssetClass}, {user.ChosenOffice.Assets[i].Brand}, {user.ChosenOffice.Assets[i].ModelName}");
        }
        Console.WriteLine($"{user.ChosenOffice.Assets.Count() + 1}. to delete all assets in {user.ChosenOffice.Name}");
        Console.WriteLine();
        Console.WriteLine("Choose which asset you want to delete: ");
        break;
    }
}