# Malicious KeePass Plugin
I have used the [KeePassHttp Plugin](https://github.com/pfn/keepasshttp) to demonstrate this PoC. I used the [KeePassHttp.plgx](https://github.com/pfn/keepasshttp/blob/master/KeePassHttp.plgx) file. It could quite easily be any other plugin.

I tested the PoC against KeePass Password Safe 2.49 (64-bit).

## The Plugin
The plugin itself isn't anything particularly complex, but the way in which KeePass compiles plugins and caches the compiled DLL is what interested me.

If you add a `.plgx` file to the KeePass Plugins directory, when KeePass is next started it will compile the file. You need elevated privileges to write to the Plugin directory which means you **cannot** just drop malicious plugins via a phishing attack or some other malware. The plugin is shown below:

![The Plugin Directory](https://github.com/plackyhacker/Malicious-KeePass-Plugin/blob/main/images/Plugin.png?raw=true)

You could try and trick users in to dropping malicious plugins here, but there's an easier way.

## The Compiled Plugin
The compiled plugin is written to the users `AppData` folder, which is **writable** without a privileged account. It isn't inconceivable that a phishing attack or some other malware could drop a malcious plugin in to this directory:

![Compiled Plugin](https://github.com/plackyhacker/Malicious-KeePass-Plugin/blob/main/images/Compiled_Plugin.png?raw=true)

All we need to do is compile the malicious plugin as a DLL and drop it over the top of the compiled `KeePassHttp.dll` file.

## Exfiltration
The next time the user opens KeePass, we can exfiltrate the master key using the malicious plugin:

![Master Key](https://github.com/plackyhacker/Malicious-KeePass-Plugin/blob/main/images/Exfiltrated_Pass.png?raw=true)

## Alleviate User Suspicions
It is very likely that a user will miss their favourite plugin when you have overwritten it with your own. You could download the source code for the target plugin, ammend it to suit your operational needs and ensure the existing functionality is not altered.
