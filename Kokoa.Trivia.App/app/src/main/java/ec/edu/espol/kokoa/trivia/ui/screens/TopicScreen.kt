package ec.edu.espol.kokoa.trivia.ui.screens

import androidx.compose.foundation.layout.Arrangement
import androidx.compose.foundation.layout.Box
import androidx.compose.foundation.layout.Column
import androidx.compose.foundation.layout.Row
import androidx.compose.foundation.layout.Spacer
import androidx.compose.foundation.layout.fillMaxWidth
import androidx.compose.foundation.layout.height
import androidx.compose.foundation.layout.padding
import androidx.compose.foundation.lazy.LazyColumn
import androidx.compose.foundation.lazy.items
import androidx.compose.material.icons.Icons
import androidx.compose.material.icons.automirrored.filled.KeyboardArrowRight
import androidx.compose.material3.CircularProgressIndicator
import androidx.compose.material3.ElevatedCard
import androidx.compose.material3.Icon
import androidx.compose.material3.MaterialTheme
import androidx.compose.material3.Text
import androidx.compose.runtime.Composable
import androidx.compose.runtime.collectAsState
import androidx.compose.runtime.getValue
import androidx.compose.ui.Alignment
import androidx.compose.ui.Modifier
import androidx.compose.ui.unit.dp
import androidx.compose.ui.unit.sp
import androidx.hilt.navigation.compose.hiltViewModel
import androidx.navigation.NavController
import ec.edu.espol.kokoa.trivia.ui.Screen
import ec.edu.espol.kokoa.trivia.ui.composables.Logo
import ec.edu.espol.kokoa.trivia.ui.viewmodels.TopicsViewModel

@Composable
fun TopicScreen(
    navController: NavController,
    modifier: Modifier = Modifier,
    viewModel: TopicsViewModel = hiltViewModel()
) {
    val topics by viewModel.topics.collectAsState()
    val loading by viewModel.loading.collectAsState()

    Column(
        modifier = modifier
            .padding(16.dp)
    ) {
        Text(
            text = "Escoge un tema",
            fontSize = 20.sp,
        )

        Spacer(
            modifier = Modifier.height(16.dp)
        )

        if (loading) {
            Box(
                modifier = Modifier.fillMaxWidth(),
                contentAlignment = Alignment.Center
            ) {
                CircularProgressIndicator()
            }
        }

        LazyColumn(
            verticalArrangement = Arrangement.spacedBy(6.dp)
        ) {
            items(topics) { topic ->
                ElevatedCard(
                    modifier = Modifier
                        .fillMaxWidth()
                        .padding(4.dp),
                    onClick = {
                        navController.navigate(Screen.Questions + "/${topic.id}/${topic.name}")
                    }
                ) {
                    Row(
                        modifier = Modifier
                            .fillMaxWidth()
                            .padding(16.dp),
                        horizontalArrangement = Arrangement.SpaceBetween
                    ) {
                        Text(
                            text = topic.name,
                            fontSize = 16.sp
                        )
                        Icon(
                            imageVector = Icons.AutoMirrored.Filled.KeyboardArrowRight,
                            contentDescription = "right arrow",
                            tint = MaterialTheme.colorScheme.primary,
                        )
                    }
                }
            }
        }

        Logo()
    }
}