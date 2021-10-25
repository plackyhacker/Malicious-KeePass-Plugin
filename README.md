# Malicious KeePass Plugin
I have used the [KeePassHttp Plugin](https://github.com/pfn/keepasshttp) to demonstrate this PoC. I used the [KeePassHttp.plgx](https://github.com/pfn/keepasshttp/blob/master/KeePassHttp.plgx) file. It quite easily have been any other plugin.

I tested the PoC against KeePass Password Safe 2.49 (64-bit).

## The Plugin
The plugin itself isn't anything particularly complex, but the way in which KeePass compiles plugins and caches the compiled DLL is what interested me.

If you add a `.plgx` file to the KeePass Plugins directory, when KeePass is next started it will compile the file. You need elevated privileges to write to the Plugin directory which means you can't just drop malicious plugins via a phishing attack. The plugin is shown below:

![The Plugin Directory](https://github.com/plackyhacker/Malicious-KeePass-Plugin/blob/main/images/Plugin.png?raw=true)

## The Compiled Plugin
The compiled plugin is written to the users `AppData` folder, which is writable without a privileged account. It isn't inconceivable that a phishing attack or some other malware could drop a malcious plugin in to this directory:

![Compiled Plugin](https://github.com/plackyhacker/Malicious-KeePass-Plugin/blob/main/images/Compiled_Plugin.png?raw=true)

## Exfiltration
The next time the user opens KeePass, we can exfiltrate the master key using the malicious plugin:

![Master Key](https://github.com/plackyhacker/Malicious-KeePass-Plugin/blob/main/images/Exfiltrated_Pass.png?raw=true)
