
HS.ZenFolio is a collection of utilities which interact with the ZenFolio online photo hosting service.

Comprises various projects aimed at different aspects of interaction with Zenfolio via its web service API.

Initial project added is a wrapper around the Zenfolio web service API designed to make authentication (and re-authentication when it times out) transparent.

This project presents a WCF service, unlike the examples provided by Zenfolio which use WS-.

It allows selection of plain or SSL transport.

Example usage:

``` using (var client = new Client(userName, password, SecurityMode.TransportEncrypted)) { var profile = client.LoadPrivateProfile();

var rootGroup = client.LoadGroupHierarchy(profile.LoginName);

Console.WriteLine(rootGroup.Title);
} ```
