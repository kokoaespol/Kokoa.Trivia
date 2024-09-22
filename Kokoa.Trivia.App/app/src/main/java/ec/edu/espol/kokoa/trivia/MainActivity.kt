package ec.edu.espol.kokoa.trivia

import android.os.Bundle
import androidx.activity.ComponentActivity
import androidx.activity.compose.setContent
import androidx.activity.enableEdgeToEdge
import androidx.compose.animation.AnimatedContentTransitionScope
import androidx.compose.animation.core.tween
import androidx.compose.animation.slideInHorizontally
import androidx.compose.animation.slideOutHorizontally
import androidx.compose.foundation.layout.fillMaxSize
import androidx.compose.foundation.layout.padding
import androidx.compose.material3.Scaffold
import androidx.compose.ui.Modifier
import androidx.navigation.NavType
import androidx.navigation.compose.NavHost
import androidx.navigation.compose.composable
import androidx.navigation.compose.rememberNavController
import androidx.navigation.navArgument
import dagger.hilt.android.AndroidEntryPoint
import ec.edu.espol.kokoa.trivia.ui.Screen
import ec.edu.espol.kokoa.trivia.ui.screens.QuestionScreen
import ec.edu.espol.kokoa.trivia.ui.screens.TopicScreen
import ec.edu.espol.kokoa.trivia.ui.screens.WelcomeScreen
import ec.edu.espol.kokoa.trivia.ui.theme.KokoaTriviaAppTheme


@AndroidEntryPoint
class MainActivity : ComponentActivity() {
    override fun onCreate(savedInstanceState: Bundle?) {
        super.onCreate(savedInstanceState)
        enableEdgeToEdge()
        setContent {

            val navController = rememberNavController()

            KokoaTriviaAppTheme {
                Scaffold(modifier = Modifier.fillMaxSize()) { innerPadding ->
                    NavHost(
                        navController = navController,
                        startDestination = Screen.Welcome
                    ) {
                        composable(Screen.Welcome) {
                            WelcomeScreen(
                                navController,
                                modifier = Modifier.padding(innerPadding)
                            )
                        }
                        composable(Screen.Topics) {
                            TopicScreen(
                                navController,
                                modifier = Modifier.padding(innerPadding)
                            )
                        }
                        composable(
                            route = Screen.Questions + "/{topicId}/{topicName}",
                            arguments = listOf(
                                navArgument("topicId") { type = NavType.IntType },
                                navArgument("topicName") { type = NavType.StringType }
                            ),
                            enterTransition = {
                                slideIntoContainer(
                                    AnimatedContentTransitionScope.SlideDirection.Left,
                                    tween(700)
                                )
                            },
                            exitTransition = {
                                slideOutOfContainer(
                                    AnimatedContentTransitionScope.SlideDirection.Right,
                                    tween(700)
                                )
                            }
                        ) { backStackEntry ->
                            QuestionScreen(
                                backStackEntry.arguments?.getInt("topicId") ?: 0,
                                backStackEntry.arguments?.getString("topicName") ?: "",
                                navController,
                                modifier = Modifier.padding(innerPadding)
                            )
                        }
                    }
                }
            }
        }
    }
}
