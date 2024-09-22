package ec.edu.espol.kokoa.trivia.ui.screens

import android.widget.Toast
import androidx.compose.foundation.background
import androidx.compose.foundation.border
import androidx.compose.foundation.clickable
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material3.Button
import androidx.compose.material3.ButtonDefaults
import androidx.compose.material3.ElevatedCard
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.LaunchedEffect
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Modifier
import androidx.compose.ui.graphics.Color
import androidx.compose.ui.platform.LocalContext
import androidx.compose.ui.text.SpanStyle
import androidx.compose.ui.text.buildAnnotatedString
import androidx.compose.ui.text.font.FontWeight
import androidx.compose.ui.text.withStyle
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.navigation.NavController
import ec.edu.espol.kokoa.trivia.ui.Screen
import ec.edu.espol.kokoa.trivia.ui.composables.Logo
import ec.edu.espol.kokoa.trivia.ui.viewmodels.QuestionsViewModel
import kotlinx.coroutines.flow.collectLatest

@Composable
fun QuestionScreen(
    topicId: Int,
    topicName: String,
    navController: NavController,
    viewModel: QuestionsViewModel = hiltViewModel(),
    modifier: Modifier = Modifier
) {
    val context = LocalContext.current

    val questions by viewModel.questions.collectAsState()
    val chosenOptions by viewModel.chosenOptions.collectAsState()
    val incorrectAnswers by viewModel.incorrectAnswers.collectAsState()
    val correctAnswers by viewModel.correctAnswers.collectAsState()

    val isChosen = { question: Int, option: Int ->
        chosenOptions[question] == option
    }

    val optionBackgroundColor = @Composable { question: Int, option: Int ->
        if (incorrectAnswers.contains(option))
            Color.Red.copy(alpha = 0.3f)
        else if (correctAnswers.contains(option))
            MaterialTheme.colorScheme.primary.copy(alpha = 0.3f)
        else if (isChosen(question, option))
            MaterialTheme.colorScheme.tertiary.copy(alpha = 0.3f)
        else
            Color.Transparent
    }

    val optionTextColor = @Composable { question: Int, option: Int ->
        if (incorrectAnswers.contains(option))
            Color.Red
        else if (correctAnswers.contains(option))
            MaterialTheme.colorScheme.primary
        else if (isChosen(question, option))
            MaterialTheme.colorScheme.tertiary
        else
            MaterialTheme.colorScheme.onSurface
    }

    LaunchedEffect(topicId) {
        viewModel.loadQuestions(topicId)
    }

    LaunchedEffect(true) {
        /* TODO: Replace with snackbar */
        viewModel.messages.collectLatest { error ->
            Toast.makeText(
                context,
                error,
                Toast.LENGTH_SHORT,
            ).show()
        }
    }

    Column(
        modifier = modifier
            .padding(16.dp)
    ) {
        Text(
            text = "Preguntas",
            fontSize = 32.sp
        )
        Text(
            text = buildAnnotatedString {
                append("Que tanto sabes sobre ")
                withStyle(
                    style = SpanStyle(
                        fontWeight = FontWeight.Bold,
                        color = MaterialTheme.colorScheme.primary,
                    ),
                ) {
                    append(topicName)
                }
                append("?")
            },
            fontSize = 24.sp
        )

        Spacer(
            modifier = Modifier.height(16.dp)
        )

        LazyColumn {
            items(questions) { question ->
                ElevatedCard(
                    modifier = Modifier.padding(vertical = 8.dp),
                ) {
                    Column(
                        modifier = Modifier.padding(8.dp)
                    ) {
                        Text(
                            text = question.title,
                            fontSize = 20.sp,
                        )

                        Spacer(
                            modifier = Modifier.height(8.dp)
                        )

                        question.options.forEach { option ->
                            Box(
                                modifier = Modifier
                                    .fillMaxWidth()
                                    .background(
                                        optionBackgroundColor(question.id, option.id),
                                        MaterialTheme.shapes.medium
                                    )
                                    .padding(4.dp)
                                    .border(
                                        1.dp,
                                        Color.LightGray,
                                        MaterialTheme.shapes.medium
                                    )
                                    .padding(8.dp)
                                    .clickable { viewModel.chooseOption(question.id, option.id) }
                            ) {
                                Text(
                                    text = option.content,
                                    color = optionTextColor(question.id, option.id)
                                )
                            }
                        }
                    }
                }
            }
        }

        Spacer(
            modifier = Modifier.height(16.dp)
        )

        Button(
            onClick = { viewModel.verifyAnswers() },
            modifier = Modifier.fillMaxWidth()
        ) {
            Text(
                text = "Verificar"
            )
        }

        Button(
            onClick = { navController.navigate(Screen.Topics) },
            modifier = Modifier.fillMaxWidth(),
            colors = ButtonDefaults.buttonColors(
                containerColor = MaterialTheme.colorScheme.secondary,
            )
        ) {
            Text(
                text = "Escoge otro tema"
            )
        }

        Logo()
    }
}