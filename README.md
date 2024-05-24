# miniproject-2024
IoT 개발자 미니프로젝트 리포지토리

## 1일차
- 조별 자리배치
- IoT 프로젝트 개요

    ![mp001](https://github.com/yongseok2312/miniproject-2024/assets/135982451/240dd9c3-0644-4b3f-9643-d464f07b97dc)
    1. IoT기기 구성 - 아두이노, 라즈베리파이 등 IoT장비들과 연결
    2. 서버 구성 - IoT기기와 통신, DB구성, 데이터 수집 앱 개발
    3. 모니터링 구성 - 실시간 모니터링/제어 앱, 전체 연결
    
- 조별 미니프로젝트
   - 5월 28일 (40시간)
    - 구체적으로 어떤 디바이스 구성, 데이터 수집, 모니터링 계획
    - 8월말 정도에 끝나는 프로젝트 일정 계획
    - **요구사항 리스트, 기능명세, UI/UX 디자인, DB설계 문서하나**로 통합
    - 5월 28일 오후 각 조별로 파워포인트/프레젠테이션 10분 발표
    - 요구사항 리스트 문서전달
    - 기능명세 문서
    - DB설계 ERD 또는 SSMS 물리적DB설계 
    - UI/UX디자인 16일(목) 내용전달

## 2일차
- 미니프로젝트
    - 프로젝트 문서 (금요일 다시)
    - UI/UX디자인 툴 설명
    - Notion ...
    - 라즈베리파이(RPi) 리셋팅, 네트워크 설정, VNC(OS UI작업)

- 클래스 토이프로젝트
    - 미니 스마트홈 연동 프로젝트
        1. 요구사항 정의, 기능명세, 일정정리
        2. UI/UX 디자인
            - RPi는 디자인 업슴
            - 데이터 수신앱
            - 모니터링 앱
        3. DB설계
        4. RPi 셋팅
        5. RPio GPIO, IoT디바이스 연결(카메라, HDT센서, RGB LED, ...)
        6. RPi 데이터 전송 파이썬 프로그래밍
        7. PC(Server) 데이터 수신 C# 프로그래밍
        8. 모니터링 앱 개발

## 3일차
- 미니프로젝트
    - 실무 프로젝트 문서
    - Jira 사용법
    - 조별로 진행

- 라즈베리파이 셋팅
    1. RPi 기본 구성 - RPi + MicroSD + Power
    2. RPi 기본 셋팅
        [x] 한글화
        - 키보드 변경
        [x] 화면 사이즈 변경(realVNC사용)
        - Pi Apps 앱설치 도우미 앱
        - Github Desktop, Vs code
        - 네트워크 확인 
        - RealVNC Server 자동 실행 설정

- 스마트홈 연동 클래스 미니프로젝트
    - RPi 셋팅... 진행 
        1. cmd 창
        2. sudo apt-get update
        3. sudo apt-get upgrade

## 4일차
- 라즈베리파이 IoT장비 설치
    [x] 라즈베리파이 카메라
    [x] GPIO HAT
    [x] 브레드보드와 연결
    [-] DHT11 센서
    [x] RGB LED 모듈
        - V - 5V 연결
        - R - GPIO4 연결
        - B - GPIO5 연결
        - G - GPIO6 연결
    [-] 서보모터

## 5일차
- 라즈베리파이 IoT장비 설치
    [x] DHT11 센서
         - GND - GND 8개중 아무데나 연결
         - VCC - 5V 연결
         - S - GPIO18연결

## 6,7일차
- 네트워크 대공사
    [x] 개인공유기, PC, 라즈베리파이

- 스마트홈 연동 클래스 미니프로젝트
    - 온습도 센서, RGB LED
    - RPi <--> Windows 통신 (MQTT)
    - WPF 모니터링 앱

- IoT 기기간 통신 방법
    - Modbus - 시리얼통신으로 데이터 전송 (완전 구식)
    - OPC UA - Modbus통신 불편한점 개선(아주 복잡)
    - MQTT - 가장 편리! AWS IoT, Awure IoT 클라우드 산업계표준으로 사용

- MQTT 통신
    - Mosquitto Broker 설치
        - mosquitto.conf : listener 1883 0,0,0,0 , allow_anonymous true
        - 방화벽 인바운드 열기
    - RPi : paho-mqtt 패키지 설치, 송신(publisher)
    - Win : MQTT.NET Nuget패키지 설치, 수신(subcriber)
