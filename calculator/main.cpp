#include <iostream>
#include <vector>
#include <fstream>
#include <string>
#include <stdexcept>

using namespace std;

// Class that handles calculation logic
class Calculator {
public:
    double calculate(double a, double b, char op) {
        switch (op) {
            case '+':
                return a + b;
            case '-':
                return a - b;
            case '*':
                return a * b;
            case '/':
                if (b != 0)
                    return a / b;
                else
                    throw runtime_error("Error: Division by zero.");
            default:
                throw invalid_argument("Error: Invalid operator.");
        }
    }
};

// Function to display welcome message
void displayWelcomeMessage() {
    cout << "===============================" << endl;
    cout << "     C++ Console Calculator    " << endl;
    cout << "===============================" << endl;
}

// Function to prompt user for input
void getUserInput(double& a, char& op, double& b) {
    cout << "\nEnter first number: ";
    cin >> a;

    cout << "Enter operator (+, -, *, /): ";
    cin >> op;

    cout << "Enter second number: ";
    cin >> b;
}

// Function to create a string record of the calculation
string buildHistoryRecord(double a, char op, double b, double result) {
    return to_string(a) + " " + op + " " + to_string(b) + " = " + to_string(result);
}

// Function to save the history to a file
void saveHistoryToFile(const vector<string>& history, const string& filename) {
    ofstream file(filename);

    for (const string& entry : history) {
        file << entry << endl;
    }

    file.close();
}

// Function to ask if the user wants another calculation
bool askToContinue() {
    string choice;
    cout << "\nDo another calculation? (yes/no): ";
    cin >> choice;

    return (choice == "yes");
}

// Function to print goodbye message
void sayGoodbye() {
    cout << "\nThank you for using the calculator!" << endl;
    cout << "History saved to 'history.txt'. Goodbye!" << endl;
}

// Main calculator logic
int main() {
    Calculator calc;
    vector<string> history;

    double a, b;
    char op;
    double result;

    displayWelcomeMessage();

    bool continueCalc = true;

    while (continueCalc) {
        try {
            getUserInput(a, op, b);

            result = calc.calculate(a, b, op);

            cout << "Result: " << result << endl;

            string record = buildHistoryRecord(a, op, b, result);
            history.push_back(record);
        }
        catch (const exception& e) {
            cout << e.what() << endl;
        }

        continueCalc = askToContinue();
    }

    saveHistoryToFile(history, "history.txt");

    sayGoodbye();

    return 0;
}
