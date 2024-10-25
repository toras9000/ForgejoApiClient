using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ForgejoApiClient.Tests.helper;

internal static class TestConstants
{
    public static string TestGpgKeyID { get; } = "CEA2ECBD5CDF17D7";
    public static string TestGpgPubKey { get; } = """
    -----BEGIN PGP PUBLIC KEY BLOCK-----

    mDMEZyfLJBYJKwYBBAHaRw8BAQdANPaBiZpBwCrcCKxLkVDczFhOFz8YkeFbFOcu
    w/pKzQm0KWZvcmdlam8tYWRtaW4gPGZvcmdlam8tYWRtaW5AZXhhbXBsZS5jb20+
    iJMEExYKADsWIQSi/hzL376XmufsVifOouy9XN8X1wUCZyfLJAIbAwULCQgHAgIi
    AgYVCgkICwIEFgIDAQIeBwIXgAAKCRDOouy9XN8X19/xAP9iWXEhyS0TkMcLDT28
    ODze+LLMQDba1bEStS4HzuoQyAEAi8GA5CIvz6/e8Pc8+QHJjy2GRyMg3m3bqusq
    9bLMhga4OARnJ8skEgorBgEEAZdVAQUBAQdA+o5ZSJF3KO2Yx9SWPvPUd9TTT758
    S6t1cr0hrh+VP2ADAQgHiHgEGBYKACAWIQSi/hzL376XmufsVifOouy9XN8X1wUC
    ZyfLJAIbDAAKCRDOouy9XN8X1wJEAQCVQIuyP0VZb+SNUBj6/n3qT2Iarxiu2R83
    +8Q2F1wCGAD9H7DHcFC+tLFnQZsGrYGcT65Jx9aOQFdckXDBiAPh6Ao=
    =vH+W
    -----END PGP PUBLIC KEY BLOCK-----
    """;
    public static string TestGpgPrivateKey { get; } = """
    -----BEGIN PGP PRIVATE KEY BLOCK-----

    lIYEZyfLJBYJKwYBBAHaRw8BAQdANPaBiZpBwCrcCKxLkVDczFhOFz8YkeFbFOcu
    w/pKzQn+BwMClkvMc3QVc1j/xC1AelAxvACqJYbaiRLsYw5Yt5oVDN63VqaSU9Q+
    5GpzVXdl6OJqoYCrqmEWFEljVHM00+V15T/1vAjWypC42PNl7vGWgLQpZm9yZ2Vq
    by1hZG1pbiA8Zm9yZ2Vqby1hZG1pbkBleGFtcGxlLmNvbT6IkwQTFgoAOxYhBKL+
    HMvfvpea5+xWJ86i7L1c3xfXBQJnJ8skAhsDBQsJCAcCAiICBhUKCQgLAgQWAgMB
    Ah4HAheAAAoJEM6i7L1c3xfX3/EA/2JZcSHJLROQxwsNPbw4PN74ssxANtrVsRK1
    LgfO6hDIAQCLwYDkIi/Pr97w9zz5AcmPLYZHIyDebduq6yr1ssyGBpyLBGcnyyQS
    CisGAQQBl1UBBQEBB0D6jllIkXco7ZjH1JY+89R31NNPvnxLq3VyvSGuH5U/YAMB
    CAf+BwMC9Tkw9qYCwwj/3VZHkfUluyFQvdeLnELn+3Ljpbo7KVn0GaE56V/52AsR
    ri097FwlLn3YuGVbiumZ0Fsq9jGAwNLg/LftIWV7OoPmT1M7i4h4BBgWCgAgFiEE
    ov4cy9++l5rn7FYnzqLsvVzfF9cFAmcnyyQCGwwACgkQzqLsvVzfF9cCRAEAlUCL
    sj9FWW/kjVAY+v596k9iGq8YrtkfN/vENhdcAhgA/R+wx3BQvrSxZ0GbBq2BnE+u
    ScfWjkBXXJFwwYgD4egK
    =dRHw
    -----END PGP PRIVATE KEY BLOCK-----
    """;
    public static string TestGpgPassphrase { get; } = "password";


    public static string TestSshPubKey { get; } = """
    ssh-ed25519 AAAAC3NzaC1lZDI1NTE5AAAAIA99VPB8502nhfarf8/Uw9Jm2+t960ifzVJIYPVE76as test
    """;

}
