
let tasks = [];

// Function to render the tasks
function renderTasks() {
    const taskList = document.getElementById('taskList');
    taskList.innerHTML = ''; 

    
    _.forEach(tasks, (task, index) => {
        const taskElement = document.createElement('div');
        taskElement.classList.add('task-item');
        
      
        const taskText = document.createElement('span');
        taskText.textContent = task.name;
        
        
        const checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.classList.add('checkbox');
        checkbox.checked = task.completed;
        
    
        checkbox.addEventListener('change', () => {
            handleTaskCompletion(index);
        });

        
        taskElement.appendChild(checkbox);
        taskElement.appendChild(taskText);

        
        taskList.appendChild(taskElement);
    });
}


function handleTaskCompletion(index) {
    try {
        if (index >= tasks.length || index < 0) {
            throw new Error('Invalid task index.');
        }

        tasks = _.map(tasks, (task, i) => {
            if (i === index) {
                task.completed = !task.completed;
            }
            return task;
        });

        renderTasks(); 
    } catch (error) {
        console.error(error.message);
    }
}

// Function to add a task
function addTask() {
    const taskInput = document.getElementById('taskInput');
    const taskName = taskInput.value.trim();

    try {
        if (!taskName) {
            throw new Error('Task cannot be empty.');
        }

        
        tasks.push({ name: taskName, completed: false });

        // Render the tasks again
        renderTasks();

        
        taskInput.value = '';
    } catch (error) {
        alert(error.message); // Show an alert for empty task
    }
}

function searchTasks() {
    const searchInput = document.getElementById('searchInput');
    const searchQuery = searchInput.value.trim().toLowerCase();

    const filteredTasks = _.filter(tasks, (task) => {
        return task.name.toLowerCase().includes(searchQuery);
    });

    const taskList = document.getElementById('taskList');
    taskList.innerHTML = '';  

    _.forEach(filteredTasks, (task, index) => {
        const taskElement = document.createElement('div');
        taskElement.classList.add('task-item');
        
        // Task text
        const taskText = document.createElement('span');
        taskText.textContent = task.name;
        
        // Checkbox element
        const checkbox = document.createElement('input');
        checkbox.type = 'checkbox';
        checkbox.classList.add('checkbox');
        checkbox.checked = task.completed;
        
        checkbox.addEventListener('change', () => {
            handleTaskCompletion(index);
        });

        taskElement.appendChild(checkbox);
        taskElement.appendChild(taskText);
        taskList.appendChild(taskElement);
    });
}


document.getElementById('addButton').addEventListener('click', addTask);


document.getElementById('searchInput').addEventListener('input', searchTasks);


renderTasks();
