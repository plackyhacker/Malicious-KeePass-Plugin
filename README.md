# Malicious KeePass Plugin
I have used the [KeePassHttp Plugin](https://github.com/pfn/keepasshttp) to demonstrate this PoC. I used the [KeePassHttp.plgx](https://github.com/pfn/keepasshttp/blob/master/KeePassHttp.plgx) file. It could quite easily be any other plugin.

I tested the PoC against KeePass Password Safe 2.49 (64-bit). This PoC is in no way a bad reflection of the KeePass application but it does demonstrate how the master key can be exfiltrated from a default installation of KeePass.

**Note:** This attack vector can only be used against pre-compiled plugins placed in the plugins cache.

## The Plugin
The plugin itself isn't anything particularly complex, but the way in which KeePass compiles plugins and caches the compiled DLL is what interested me.

If you add a `.plgx` file to the KeePass Plugins directory, when KeePass is next started it will compile the file. You need elevated privileges to write to the Plugin directory which means you **cannot** just drop malicious plugins via a phishing attack or some other malware. Whilst this might not be a concern for a home user (they are probably logged in as an administrator anyway), it might be a concern in a corporate environment where the IT team have installed KeePass for non-privileged users. The `KeePassHttp.plgx` plugin is shown below:

![The Plugin Directory](https://github.com/plackyhacker/Malicious-KeePass-Plugin/blob/main/images/Plugin.png?raw=true)

You could try and trick users in to dropping malicious plugins here, but there is an easier way.

## The Compiled Plugin
The compiled plugin is written to the users `AppData` folder, which is **writable** without a privileged account by default. It isn't inconceivable that a phishing attack or some other malware could drop a malcious plugin in to this directory to try amd elevate privileges, or gain access to a users sensitive data:

![Compiled Plugin](https://github.com/plackyhacker/Malicious-KeePass-Plugin/blob/main/images/Compiled_Plugin.png?raw=true)

All we need to do is compile the malicious plugin as a DLL and drop it over the top of the compiled `KeePassHttp.dll` file, and ensuring the DLL is written and compiled according to the [plugin requirements](https://keepass.info/help/v2_dev/plg_index.html).

## Exfiltration
The next time the user opens KeePass, we can exfiltrate the master key using the malicious plugin:

![Master Key](https://github.com/plackyhacker/Malicious-KeePass-Plugin/blob/main/images/Exfiltrated_Pass.png?raw=true)

## Alleviate User Suspicions
It is very likely that a user will miss their favourite plugin when you have overwritten it with your own. You could download the source code for the target plugin (if it is available - or even decompile it - it is a .Net assembly). You could then ammend it to suit your own needs and ensure the existing functionality is not altered.

# Mitigation
I contacted the developer of KeePass, **Dominik Reichl**, before posting this, I wanted to make sure it wasn't an unknown attack vector. However, this is an instance where the functionality of a program might outweigh the risk, 'This cache highly improves the startup performance of KeePass'. known security 'issues' in KeePass are posted [in the KeePass knowledge base](https://keepass.info/help/kb/sec_issues.html#cfgw).

The best mitigation is to be aware of how the KeePass plugins work. If you have any concerns about this type of attack vector then the `PluginCache` folder should be avoided, instead placing pre-compiled DLL plugins in the `C:\Program Files\KeePass Password Safe 2\Plugins` protected folder. A non-privileged user cannot overwrite these by default.
