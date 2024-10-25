using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LibGit2Sharp;

namespace ForgejoApiClient.Tests.helper;

public sealed class TestTempRepo : IDisposable
{
    public TestTempRepo(string cloneUri)
    {
        this.tempDir = new TestTempDir();

        var gitPath = Repository.Clone(cloneUri, this.tempDir.Info.FullName);
        this.tempRepo = new Repository(gitPath);

        this.Auther = "test-auther";
        this.AutherMail = "test-auther@example.com";
    }

    public string Auther { get; set; }
    public string AutherMail { get; set; }

    public DirectoryInfo Info => this.tempDir.Info;
    public Repository Repo => this.tempRepo;

    public Branch Branch(string name)
    {
        var branch = this.tempRepo.CreateBranch(name);
        return branch;
    }

    public Tag LightwaightTag(string name)
    {
        var tag = this.tempRepo.ApplyTag(name);
        return tag;
    }

    public Tag AnnotatedTag(string name, string message)
    {
        var auther = new Signature(this.Auther, this.AutherMail, DateTime.Now);
        var tag = this.tempRepo.ApplyTag(name, auther, message);
        return tag;
    }

    public Commit Commit(string message, Action<DirectoryInfo> modifier)
    {
        modifier(this.Info);
        Commands.Stage(this.tempRepo, "*");
        var auther = new Signature(this.Auther, this.AutherMail, DateTime.Now);
        var commit = this.tempRepo.Commit(message, auther, auther);
        return commit;
    }

    public void Push(string user, string pass)
        => Push(this.tempRepo.Head, user, pass);

    public void Push(Branch branch, string user, string pass)
    {
        var remote = this.tempRepo.Network.Remotes.First();
        this.tempRepo.Branches.Update(branch, b => b.Remote = remote.Name, b => b.UpstreamBranch = branch.CanonicalName);
        var pushOpt = new PushOptions();
        pushOpt.CredentialsProvider += (_, _, _) => new UsernamePasswordCredentials() { Username = user, Password = pass, };
        this.tempRepo.Network.Push(branch, pushOpt);
    }

    public void Push(string refspec, string user, string pass)
    {
        var remote = this.tempRepo.Network.Remotes.First();
        var pushOpt = new PushOptions();
        pushOpt.CredentialsProvider += (_, _, _) => new UsernamePasswordCredentials() { Username = user, Password = pass, };
        this.tempRepo.Network.Push(remote, refspec, pushOpt);
    }

    public void Dispose()
    {
        this.tempRepo.Dispose();
        this.tempDir.Dispose();
    }

    private TestTempDir tempDir;
    private Repository tempRepo;
}
