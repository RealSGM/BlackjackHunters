using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TrainingCamp : MonoBehaviour
{
    public void ExitTraining()
    {
        SceneManager.LoadScene("Castle");
    }
}
