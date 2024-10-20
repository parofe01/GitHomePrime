using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using UnityEngine;


public class MainMenuControllers : NetworkBehaviour
{
    // Variables



    // GameObjects
    public NetworkManager networkManager;
    public GameObject mPanelPolicies;
    public GameObject mPanelMain;
    public GameObject mPanelHost;
    public GameObject mPanelClient;
    public GameObject mPanelHelp;
    public GameObject mLobby;
    public GameObject mMenuCamera;

    // Components
    public UnityTransport unityTransport;

    void Start()
    {
        mMenuCamera.SetActive(true);
        mPanelPolicies.SetActive(true);
        mPanelMain.SetActive(false);
        mPanelHost.SetActive(false);
        mPanelClient.SetActive(false);
        mPanelHelp.SetActive(false);
        mLobby.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner) return;
    }

    public void ClickAcceptPolicies()
    {
        mPanelPolicies.SetActive(false);
        mPanelMain.SetActive(true);
    }
    public void ClickRejectPolicies()
    {
        ClickExit();
    }

    public void ClickExit()
    {
        Application.Quit();
    }

    public void ClickHost()
    {
        mPanelMain.SetActive(false);
        mPanelHost.SetActive(true);
    }

    public void ClickClient()
    {
        mPanelMain.SetActive(false);
        mPanelClient.SetActive(true);
    }

    public void LoadLobby()
    {
        mLobby.SetActive(true);
        mPanelHost.SetActive(false);
        mPanelClient.SetActive(false);
        mMenuCamera.SetActive(false);
    }
    public void ClickHelp()
    {
        mPanelHelp.SetActive(true);
    }

    public void ClickBackMain()
    {
        mPanelMain.SetActive(true);
        mPanelHost.SetActive(false);
        mPanelClient.SetActive(false);
        mPanelHelp.SetActive(false);
    } 

    public void ClickStartHosting()
    {
        LoadLobby();
        networkManager.StartHost();
    }

    public void ClickStartClient()
    {
        LoadLobby();
        networkManager.StartClient();
    }
}
