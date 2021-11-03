using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
///  The Source file name: BasicButtonController.cs
///  Author's name: Trung Le (Kyle Hunter)
///  Student Number: 101264698
///  Program description: Global game manager script
///  Date last Modified: See GitHub
///  Revision History: See GitHub
/// </summary>
public class BasicButtonController : MonoBehaviour
{
    public void DoLoadMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void DoLoadNextScene()
    {
        StartCoroutine(Enlarge());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void DoLoadPrevScene()
    {
        StartCoroutine(Shrink());
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);
    }

    public void DoQuitApp()
    {
        StartCoroutine(Shrink());
        Application.Quit();
    }

    IEnumerator Shrink()
    {
        for (float ft = 1f; ft <= 0.1f; ft -= 0.05f)
        {
            gameObject.transform.localScale = new Vector3(ft, ft, ft);
            yield return new WaitForSeconds(2.0f);
        }
    }

    IEnumerator Enlarge()
    {
        for (float ft = 0.1f; ft <= 1f; ft += 0.05f)
        {
            gameObject.transform.localScale = new Vector3(ft, ft, ft);
            //yield return null;
            yield return new WaitForSeconds(2.0f);
        }
    }
}
