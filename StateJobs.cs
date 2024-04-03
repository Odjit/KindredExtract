/*


using KindredExtract;
using ProjectM;
using ProjectM.Network;
using Unity.Jobs;

[System.AttributeUsage(System.AttributeTargets.Interface)]
public sealed class JobProducerTypeKindredAttribute : System.Attribute
{
    /// <summary>
    ///   <para>ProducerType is the type containing a static method named "Execute" method which is the method invokes by the job system.</para>
    /// </summary>
    public System.Type ProducerType { get; }

    /// <summary>
    ///   <para></para>
    /// </summary>
    /// <param name="producerType">The type containing a static method named "Execute" method which is the method invokes by the job system.</param>
    public JobProducerTypeKindredAttribute(System.Type producerType) => this.ProducerType = producerType;
}

[JobProducerTypeKindred(typeof(IJobExtensions.JobStruct<>))]
public interface IJobKindred
{
    /// <summary>
    ///   <para>Implement this method to perform work on a worker thread.</para>
    /// </summary>
    void Execute();
}

struct PlayerStateJob : IJobKindred
{
    public User user;
    public void Execute()
    {
        ServerChatUtils.SendSystemMessageToClient(Core.EntityManager, user, "Test");
    }
}
*/