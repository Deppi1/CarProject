using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextManager : MonoBehaviour
{
    public Text headerText;
    public Text bodyText;
    public Text taskDescription;
    // Start is called before the first frame update
    public void Header()
    {
        headerText.text = "Меню";
    }

     public void BodyText()
    {
        bodyText.text = "Добро пожаловать! Выберите чем хотите заняться при помощи кнопок снизу";
    }

    public void FirstLevel()
    {
        headerText.text = "Первое задание";
        taskDescription.text = "Вам необходимо преодолеть участок трассы как можно быстрее. Для этого подходите к повороту снаружи, старайтесь оттормозиться до входа в поворот, поверните руль внутрь поворота, пройдя примерно половину поворота - открывайте на газ.";
    }

    public void SecondLevel()
    {
        headerText.text = "Второе задание";
        taskDescription.text = "Вам необходимо преодолеть участок трассы как можно быстрее. Для этого подходите к повороту снаружи, старайтесь оттормозиться до входа в поворот, поверните руль внутрь поворота, пройдя примерно половину поворота - открывайте на газ.";
    }

    public void ThirdLevel()
    {
        headerText.text = "Третье задание";
        taskDescription.text = "Вам необходимо преодолеть участок трассы как можно быстрее. Для этого подходите к повороту снаружи, старайтесь оттормозиться до входа в поворот, поверните руль внутрь поворота, пройдя примерно половину поворота - открывайте на газ.";
    }

    public void FourthLevel()
    {
        headerText.text = "Четвертое задание";
        taskDescription.text = "Вам необходимо преодолеть участок трассы как можно быстрее. Для этого подходите к повороту снаружи, старайтесь оттормозиться до входа в поворот, поверните руль внутрь поворота, пройдя примерно половину поворота - открывайте на газ.";
    }

    public void FifthLevel()
    {
        headerText.text = "Пятое задание";
        taskDescription.text = "Вам необходимо преодолеть всю трассу как можно быстрее. Для этого подходите к повороту снаружи, старайтесь оттормозиться до входа в поворот, поверните руль внутрь поворота, пройдя примерно половину поворота - открывайте на газ.";
    }

    public void SixthLevel()
    {
        taskDescription.text = "Шестое задание";
    }

    public void Footer()
    {

    }
}
