using TMPro;
using UnityEngine;

public class ExpenseTrackerUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI totalExpenseText;
    [SerializeField] private TextMeshProUGUI budgetText;

    private float totalExpense = 0f;
    private float currentBudget = 0f;

    private void Start()
    {
        RefreshUI();
    }

    public void AddExpense(float itemPrice)
    {
        totalExpense += itemPrice;
        RefreshUI();
    }

    public void SetBudget(float budgetAmount)
    {
        currentBudget = budgetAmount;
        RefreshUI();
    }

    private void RefreshUI()
    {
        totalExpenseText.text = totalExpense.ToString() + "$";
        budgetText.text = currentBudget.ToString() + "$";
    }
}
