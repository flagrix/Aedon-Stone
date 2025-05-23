using Photon.Pun;
using UnityEngine;

public class ForAllNodes : MonoBehaviour, IPunInstantiateMagicCallback
{
    public Node assignedNode;

    public void OnPhotonInstantiate(PhotonMessageInfo info)
    {
        object[] instantiationData = info.photonView.InstantiationData;
        if (instantiationData != null && instantiationData.Length > 0)
        {
            string nodeName = (string)instantiationData[0];

            // Trouver le node correspondant sur ce client
            Node[] allNodes = FindObjectsOfType<Node>();
            foreach (Node node in allNodes)
            {
                if (node.name == nodeName)
                {
                    assignedNode = node;
                    node.turret = gameObject;
                    break;
                }
            }

            Debug.Log("Canon assigné à " + nodeName);
        }
    }
}


