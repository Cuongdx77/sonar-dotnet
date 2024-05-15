pipeline {
  environment {
    dockerimagename = "dxcuong206/dotnet-test"
    dockerImage = ""
  }
  agent none
  stages {
    stage('Checkout Source') {
      agent { label 'agent1' }
      steps {
        git branch: 'master', credentialsId: 'GitHubCred', url: 'https://github.com/Cuongdx77/Dotnet-Test.git'
      }
    }

    stage('Sonarqube') {
      agent { label 'agent3' }
      environment {
        scannerHome = tool 'sonarqube_scanner'
      }
      steps {
        withSonarQubeEnv('Sonarqube_server') {
          sh 'cd /root/ETicaretAPI'
          sh 'docker build -f Dockerfile-Sonar -t dotnet-sonarscan:03 --rm .'
        }
      }
    }

    stage('Quality Gate Check') {
      agent { label 'agent1' }
      steps {
        script {
          def response = sh(script: 'curl -u "sqa_1930a831282b897e091d3074560eb2ef2e0bf5c8:" "http://10.26.2.215:9000/api/qualitygates/project_status?projectKey=test-sonarqube1" | jq -r ".projectStatus.status"', returnStdout: true).trim()
          echo "Quality Gate Status: ${response}"
          if ("${response}" == 'ERROR') {
            currentBuild.result = 'ABORTED'
            error('Job Aborted due to Quality Gate failure')
          }
        }
      }
    }
    stage('Build image') {
      agent { label 'agent1' }
      steps {
        sh "docker build -f Dockerfile -t dotnet-test:03 ."
      }
    }
  }
 }
