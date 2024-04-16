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
        headerText.text = "����";
    }

     public void BodyText()
    {
        bodyText.text = "����� ����������! �������� ��� ������ �������� ��� ������ ������ �����";
    }

    public void FirstLevel()
    {
        headerText.text = "������ �������";
        taskDescription.text = "��� ���������� ���������� ������� ������ ��� ����� �������. ��� ����� ��������� � �������� �������, ���������� ������������� �� ����� � �������, ��������� ���� ������ ��������, ������ �������� �������� �������� - ���������� �� ���.";
    }

    public void SecondLevel()
    {
        headerText.text = "������ �������";
        taskDescription.text = "��� ���������� ���������� ������� ������ ��� ����� �������. ��� ����� ��������� � �������� �������, ���������� ������������� �� ����� � �������, ��������� ���� ������ ��������, ������ �������� �������� �������� - ���������� �� ���.";
    }

    public void ThirdLevel()
    {
        headerText.text = "������ �������";
        taskDescription.text = "��� ���������� ���������� ������� ������ ��� ����� �������. ��� ����� ��������� � �������� �������, ���������� ������������� �� ����� � �������, ��������� ���� ������ ��������, ������ �������� �������� �������� - ���������� �� ���.";
    }

    public void FourthLevel()
    {
        headerText.text = "��������� �������";
        taskDescription.text = "��� ���������� ���������� ������� ������ ��� ����� �������. ��� ����� ��������� � �������� �������, ���������� ������������� �� ����� � �������, ��������� ���� ������ ��������, ������ �������� �������� �������� - ���������� �� ���.";
    }

    public void FifthLevel()
    {
        headerText.text = "����� �������";
        taskDescription.text = "��� ���������� ���������� ��� ������ ��� ����� �������. ��� ����� ��������� � �������� �������, ���������� ������������� �� ����� � �������, ��������� ���� ������ ��������, ������ �������� �������� �������� - ���������� �� ���.";
    }

    public void SixthLevel()
    {
        taskDescription.text = "������ �������";
    }

    public void Footer()
    {

    }
}
